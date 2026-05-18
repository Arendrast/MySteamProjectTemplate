using System;
using Cysharp.Threading.Tasks;
using Modules.AppModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.AppModule.Runtime.Shared.Scripts.Systems.UISystem.PausePopup
{
    public class PausePopupFactory : IPersistentFactory, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly HashedAssetProvider _hashedAssetProvider;
        private readonly SettingsPopupFactory _settingsPopupFactory;
        private readonly IInputService _inputService;
        private readonly TimeScaleRepository _timeScaleRepository;

        private const string PausePopupAssetId =
#if ADDRESSABLES
            "PausePopup";
#else
        "CoreModule/Runtime/Client/Loadable/Prefabs/PausePopup";
#endif

        public PausePopupFactory(EventBus eventBus, HashedAssetProvider hashedAssetProvider,
             SettingsPopupFactory settingsPopupFactory,
            IInputService inputService, TimeScaleRepository timeScaleRepository)
        {
            _eventBus = eventBus;
            _hashedAssetProvider = hashedAssetProvider;
            _settingsPopupFactory = settingsPopupFactory;
            _inputService = inputService;
            _timeScaleRepository = timeScaleRepository;
        }

        public void Dispose()
        {
            DisposeAsync().Forget();
        }

        public async UniTask DisposeAsync()
        {
            await _hashedAssetProvider.DisposeAsync();
        }

        public async UniTask<PausePopupController> GetPausePopupControllerAsync()
        {
            return await _hashedAssetProvider
                .GetControllerAsync<PausePopupController, PausePopupSerializableComponents>(
                    PausePopupAssetId,
                    popup =>
                    {
                        _hashedAssetProvider.RegisterAndGetSingleByType(new PausePopupController(_inputService,
                            _timeScaleRepository, popup, _eventBus, _settingsPopupFactory));
                        return UniTask.CompletedTask;
                    }, 
                    shouldMakeDontDestroyOnLoad: true
                );
        }
    }
}