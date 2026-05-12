using System;
using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup
{
    public class SettingsPopupFactory : IPersistentFactory, IDisposable
    {
        private readonly HashedAssetProvider _hashedAssetProvider;
        private readonly MouseSensitivityRepository _mouseSensitivityRepository;
        private readonly EventBus _eventBus;

        private const string SettingsPopupAssetId = "SettingsPopup";

        public SettingsPopupFactory(HashedAssetProvider hashedAssetProvider,
            MouseSensitivityRepository mouseSensitivityRepository,
            EventBus eventBus)
        {
            _hashedAssetProvider = hashedAssetProvider;
            _mouseSensitivityRepository = mouseSensitivityRepository;
            _eventBus = eventBus;
        }

        public void Dispose()
        {
            DisposeAsync().Forget();
        }

        public async UniTask DisposeAsync()
        {
            await _hashedAssetProvider.DisposeAsync();
        }

        public async UniTask<SettingsPopupController> GetSettingsPopupControllerAsync()
        {
            return await _hashedAssetProvider
                .GetControllerAsync<SettingsPopupController, SettingsPopupSerializableComponents>(
                    SettingsPopupAssetId,
                    popup =>
                    {
                        _hashedAssetProvider.RegisterAndGetSingleByType(new SettingsPopupController(popup,
                            _mouseSensitivityRepository, _eventBus));
                        return UniTask.CompletedTask;
                    }, shouldMakeDontDestroyOnLoad: true);
        }
    }
}