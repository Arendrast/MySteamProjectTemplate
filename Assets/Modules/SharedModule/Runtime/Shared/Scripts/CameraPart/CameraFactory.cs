using System;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class CameraFactory : IPersistentFactory, IDisposable
    {
        private readonly ConfigsProviderService _configsProvider;
        private readonly HashedAssetProvider _hashedAssetProvider;
        private readonly IInputProvider _inputProvider;
        private readonly MouseSensitivityRepository _mouseSensitivityRepository;

        private const string MainCameraAssetId =
#if ADDRESSABLES
            "MainCamera";
#else
        "CoreModule/Runtime/Loadable/Prefabs/MainCamera";
#endif

        public CameraFactory(
            HashedAssetProvider hashedAssetProvider,
            IInputProvider inputProvider,
            ConfigsProviderService configsProvider, MouseSensitivityRepository mouseSensitivityRepository)
        {
            _hashedAssetProvider = hashedAssetProvider;
            _inputProvider = inputProvider;
            _configsProvider = configsProvider;
            _mouseSensitivityRepository = mouseSensitivityRepository;
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
            var moveConfig = await _configsProvider.GetConfigAsync<MovementConfig>();

            return await _hashedAssetProvider.GetControllerAsync<CameraComponents, CameraSerializableComponents>(
                MainCameraAssetId,
                instance =>
                {
                    _hashedAssetProvider.RegisterAndGetSingleByType(
                        new CameraComponents(
                            new FPSCameraController(
                                instance.FPSCameraSerializableComponents,
                                _inputProvider,
                                new CameraControllerData(() =>
                                    moveConfig.RotationSpeed * _mouseSensitivityRepository.CurrentSensitivity),
                                instance[CameraParentType.Move]),
                            instance, new FollowPositionController(instance.transform),
                            new FollowRotationController(instance.transform)));
                    return UniTask.CompletedTask;
                },
                shouldMakeDontDestroyOnLoad: true);
        }
    }
}