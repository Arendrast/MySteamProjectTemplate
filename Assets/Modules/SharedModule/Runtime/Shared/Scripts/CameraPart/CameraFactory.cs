using System;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class CameraFactory : IPersistentFactory, IDisposable
    {
        private readonly ConfigsProviderService _configsProvider;
        private readonly HashedAssetProvider _hashedAssetProvider;
        private readonly IInputService _inputService;
        private readonly MouseSensitivityRepository _mouseSensitivityRepository;
        private readonly UpdateObserversService _updateObserversService;

        private const string MainCameraAssetId =
#if ADDRESSABLES
            "MainCamera";
#else
        "CoreModule/Runtime/Loadable/Prefabs/MainCamera";
#endif

        public CameraFactory(
            HashedAssetProvider hashedAssetProvider,
            IInputService inputService,
            ConfigsProviderService configsProvider, MouseSensitivityRepository mouseSensitivityRepository, UpdateObserversService updateObserversService)
        {
            _hashedAssetProvider = hashedAssetProvider;
            _inputService = inputService;
            _configsProvider = configsProvider;
            _mouseSensitivityRepository = mouseSensitivityRepository;
            _updateObserversService = updateObserversService;
        }

        public void Dispose()
        {
            DisposeAsync().Forget();
        }

        public async UniTask DisposeAsync()
        {
            await _hashedAssetProvider.DisposeAsync();
        }

        public async UniTask<CameraComponents> GetCreatedMainCameraAsync()
        {
            return await _hashedAssetProvider.GetControllerAsync<CameraComponents, CameraSerializableComponents>(
                MainCameraAssetId,
                instance =>
                {
                    _updateObserversService.TryAddOrGetUpdateObserver(
                        instance.TwoDCameraSerializableComponents.gameObject, UpdateType.LateUpdate, out var observer);
                    
                    _hashedAssetProvider.RegisterAndGetSingleByType(
                        new CameraComponents(
                            new TwoDCameraMovementController(
                                instance.TwoDCameraSerializableComponents,
                                _inputService,
                                new CameraControllerData(),
                                instance[CameraParentType.Move], observer),
                            instance, new FollowPositionController(instance.transform),
                            new FollowRotationController(instance.transform)));
                    return UniTask.CompletedTask;
                },
                shouldMakeDontDestroyOnLoad: true);
        }
    }
}