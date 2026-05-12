using System;
using Cysharp.Threading.Tasks;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.LevelModule.Runtime.Shared.Scripts;
using Modules.NetworkModule.Runtime.Shared.Scripts.Scene;
using Modules.SharedModule.Runtime.Client.Scripts.GameStateMachine;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using Steamworks;
using UnityEngine;

namespace Modules.CoreModule.Runtime.Server.Scripts.Infrastructure.Services
{
    public class MatchServerGameSubState : IMatchServerGameSubState, IMatchServerService
    {
        private readonly SteamEditorConfig _steamEditorConfig;
        private readonly NetworkManager _networkManager;
        private readonly ServerManager _serverManager;
        private readonly ClientManager _clientManager;
        private readonly EventBus _eventBus;
        private readonly ServerSceneManagementService _serverSceneManagementService;
        private readonly ISharedSubscribingMediator _subscribingMediator;
        private readonly LevelZoneFactory _levelZoneFactory;
        private readonly NetworkCountersSynchronizerBehaviourRepository _networkCountersSynchronizerBehaviourRepository;
        private readonly NetworkCountersSynchronizerBehaviourFactory _networkCountersSynchronizerBehaviourFactory;

        public MatchServerGameSubState(SteamEditorConfig steamEditorConfig, NetworkManager networkManager,
            ServerManager serverManager, ClientManager clientManager,
            ServerSceneManagementService serverSceneManagementService, ISharedSubscribingMediator subscribingMediator,
            NetworkCountersSynchronizerBehaviourRepository networkCountersSynchronizerBehaviourRepository,
            NetworkCountersSynchronizerBehaviourFactory networkCountersSynchronizerBehaviourFactory, LevelZoneFactory levelZoneFactory)
        {
            _steamEditorConfig = steamEditorConfig;
            _networkManager = networkManager;
            _serverManager = serverManager;
            _clientManager = clientManager;
            _serverSceneManagementService = serverSceneManagementService;
            _subscribingMediator = subscribingMediator;
            _networkCountersSynchronizerBehaviourRepository = networkCountersSynchronizerBehaviourRepository;
            _networkCountersSynchronizerBehaviourFactory = networkCountersSynchronizerBehaviourFactory;
            _levelZoneFactory = levelZoneFactory;
        }

        public void Dispose()
        {
            Exit();
        }

        public async UniTask EnterAsync(string sceneName, LevelConfig levelConfig)
        {
            await InitializeServerSideAsync(sceneName);

            _networkCountersSynchronizerBehaviourRepository.Behaviour =
                await _networkCountersSynchronizerBehaviourFactory.GetSpawnedSynchronizer();

            await _levelZoneFactory.InitializeStartLevelZones(levelConfig);
        }

        private async UniTask InitializeServerSideAsync(string sceneName)
        {
            if (_steamEditorConfig.ShouldUseSteam)
            {
                LobbyData.CreatePublicSession(_networkManager.TransportManager.Transport.GetMaximumClients(),
                    StartHost);
            }
            else
            {
                StartHost();
            }

            await _serverSceneManagementService.LoadNewStackedGameSceneAsync(sceneName);
            await UniTask.WaitWhile(() => !_clientManager.GetOwnerConnection().IsValid);

            return;

            void StartHost(EResult result = default, LobbyData data = default, bool ioError = false)
            {
                try
                {
                    _serverManager.StopConnection(true);
                    _clientManager.StopConnection();
                }
                catch (Exception exception)
                {
                    Debug.Log(exception);
                }

                if (!_serverManager.StartConnection() || !_clientManager.StartConnection("localhost"))
                {
                    _eventBus.Fire(new EnterGameStateEvent(GameStateType.MainMenu));
                    return;
                }

                _subscribingMediator.Subscribe();
            }
        }


        private void Exit()
        {
        }
    }
}