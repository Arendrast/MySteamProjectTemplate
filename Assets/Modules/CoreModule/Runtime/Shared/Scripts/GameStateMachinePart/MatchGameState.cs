using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.LoadingPopup;
using Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.PausePopup;
using Modules.LevelModule.Runtime.Shared.Scripts;
using Modules.PlayerModule.Runtime.Shared.Scripts.Operator;
using Modules.SharedModule.Runtime.Shared.Scripts.CameraPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.GameStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.Holders;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Repository;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Modules.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart
{
    public class MatchGameState : IGameState
    {
        private bool _addedSpawnableNetworkObjects;
        
        private readonly ConfigsProviderService _configsProvider;
        private readonly InputActions _inputActions;
        private readonly ClientManager _clientManager;
        private readonly ServerManager _serverManager;
        private readonly CameraFactory _cameraFactory;
        private readonly List<IRepository> _allRepositories;
        private readonly PausePopupFactory _pausePopupFactory;
        private readonly MatchSharedServicesScope _matchSharedServicesScope;
        private readonly IMatchServerServicesScope _matchServerServicesScope;
        private readonly IMatchClientServicesScope _matchClientServicesScope;
        private readonly LoadingPopupFactory _loadingPopupFactory;
        private readonly IsOperatorRepository _isOperatorRepository;
      
        public MatchGameState(
            InputActions inputActions,
            ConfigsProviderService configsProvider,
            ClientManager clientManager,
            ServerManager serverManager,
            CameraFactory cameraFactory,
            PausePopupFactory pausePopupFactory,
            MatchSharedServicesScope matchSharedServicesScope,
            LoadingPopupFactory loadingPopupFactory,
            IsOperatorRepository isOperatorRepository,
            IMatchServerServicesScope matchServerServicesScope, IMatchClientServicesScope matchClientServicesScope)
        {
            _configsProvider = configsProvider;
            _clientManager = clientManager;
            _serverManager = serverManager;
            _cameraFactory = cameraFactory;
            _pausePopupFactory = pausePopupFactory;
            _matchSharedServicesScope = matchSharedServicesScope;
            _loadingPopupFactory = loadingPopupFactory;
            _isOperatorRepository = isOperatorRepository;
            _matchServerServicesScope = matchServerServicesScope;
            _matchClientServicesScope = matchClientServicesScope;
            _inputActions = inputActions;
        }

        public async UniTask EnterAsync(IGameStateEnterData data)
        {
            _inputActions.Disable();

            if (data is not EnterMatchGameStateData enterMatchGameStateData)
                throw new Exception("Incorrect data type");

            if (!_addedSpawnableNetworkObjects)
            {
                await NetworkAssetsLoader.AddNetworkObjectsToSpawnableObjects();
                _addedSpawnableNetworkObjects = true;
            }
            
            var levelsConfig = await _configsProvider.GetConfigAsync<LevelsConfig>();
            var levelConfig = levelsConfig.LevelsConfigs[enterMatchGameStateData.TargetLevelIndex];
            var sceneName = Application.isEditor ? levelConfig.SceneName : ScenesNamesHolder.Game;

            var isOperator = _isOperatorRepository.IsOperator;
            
            await _matchSharedServicesScope.CustomBuildAsync();

            LifetimeScope targetScope = enterMatchGameStateData.IsHost
                ? (LifetimeScope) _matchServerServicesScope
                : (LifetimeScope) _matchClientServicesScope;
            
            await targetScope.CustomBuildAsync();
            
            if (enterMatchGameStateData.IsHost)
            {
                await targetScope.Container.Resolve<IMatchServerGameSubState>().EnterAsync(sceneName, levelConfig);
            }
            else
            {
                await targetScope.Container.Resolve<IMatchClientGameSubState>().EnterAsync(isOperator,
                    enterMatchGameStateData.HostSteamId, sceneName);
            }

            var subscribingMediator = targetScope.Container.Resolve<ISharedSubscribingMediator>();
            await targetScope.Container.Resolve<MatchSharedGameSubState>().EnterAsync(isOperator, subscribingMediator);

            (await _pausePopupFactory.GetPausePopupControllerAsync()).TryClosePopup();
            await _loadingPopupFactory.DisposeAsync();
        }

        public async UniTask ExitAsync()
        {
            CursorSwitchTools.TryEnableCursor();

            try
            {
                _serverManager.StopConnection(true);
                _clientManager.StopConnection();
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }

            (await _pausePopupFactory.GetPausePopupControllerAsync()).TryClosePopup();

            var mainCamera = (await _cameraFactory.GetCreatedMainCameraAsync());

            mainCamera.Dispose();

            _matchSharedServicesScope.DisposeCore();
            _matchServerServicesScope.DisposeCore();
            _matchClientServicesScope.DisposeCore();
        }
    }
}