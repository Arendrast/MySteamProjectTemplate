using System;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.LevelModule.Runtime.Shared.Scripts;
using Modules.PlayerModule.Runtime.Shared.Scripts.Operator;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using Steamworks;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.MainMenuPopup
{
    public class MainMenuPopupFactory : IPersistentFactory, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly HashedAssetProvider _hashedAssetProvider;
        private readonly ConfigsProviderService _configsProviderService;
        private readonly SteamEditorConfig _steamEditorConfig;
        private readonly IsOperatorRepository _isOperatorRepository;
        private readonly ServerManager _serverManager;

        private const string MainMenuPopupAssetId =
#if ADDRESSABLES
            "MainMenuPopup";
#else
        "CoreModule/Runtime/Client/Loadable/Prefabs/LoadingGamePopup";
#endif

        public MainMenuPopupFactory(EventBus eventBus, HashedAssetProvider hashedAssetProvider,
            ConfigsProviderService configsProviderService, SteamEditorConfig steamEditorConfig,
            IsOperatorRepository isOperatorRepository, ServerManager serverManager)
        {
            _eventBus = eventBus;
            _hashedAssetProvider = hashedAssetProvider;
            _configsProviderService = configsProviderService;
            _steamEditorConfig = steamEditorConfig;
            _isOperatorRepository = isOperatorRepository;
            _serverManager = serverManager;
        }

        public void Dispose()
        {
            DisposeAsync().Forget();
        }

        public async UniTask DisposeAsync()
        {
            await _hashedAssetProvider.DisposeAsync();
        }

        public async UniTask<MainMenuPopupController> GetMainMenuPopupControllerAsync()
        {
            return await _hashedAssetProvider
                .GetControllerAsync<MainMenuPopupController, MainMenuPopupSerializableComponents>(
                    MainMenuPopupAssetId,
                    async popup =>
                    {
                        _hashedAssetProvider.RegisterAndGetSingleByType(new MainMenuPopupController(popup,
                            await _configsProviderService.GetConfigAsync<LevelsConfig>(), _eventBus,
                            _steamEditorConfig.ShouldUseSteam ? SteamUser.GetSteamID().ToString() : null,
                            _isOperatorRepository, _serverManager));
                    },
                    shouldMakeDontDestroyOnLoad: true
                );
        }
    }
}