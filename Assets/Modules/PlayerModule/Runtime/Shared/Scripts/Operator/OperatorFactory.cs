using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.CameraPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Loading;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Operator
{
    public class OperatorFactory : IMatchSharedFactory
    {
        private readonly IAssetLoader _assetLoader;
        private readonly CameraFactory _cameraFactory;
        private readonly IInputService _inputService;
        private readonly UpdateObserversService _updateObserversService;

        public OperatorFactory(IAssetLoader assetLoader, CameraFactory cameraFactory, IInputService inputService,
            UpdateObserversService updateObserversService)
        {
            _assetLoader = assetLoader;
            _cameraFactory = cameraFactory;
            _inputService = inputService;
            _updateObserversService = updateObserversService;
        }

        public async UniTask<OperatorMovementController> GetCreatedOperatorMovementControllerAsync()
        {
            var operatorSerializableComponents =
                await AssetProvider.InstantiateAsync<OperatorSerializableComponents>("Operator", _assetLoader);

            var camera = await _cameraFactory.GetCreatedMainCameraAsync();

            operatorSerializableComponents.transform.SetParent(camera.SerializableComponents.Camera.transform);

            _updateObserversService.TryAddOrGetUpdateObserver(operatorSerializableComponents.gameObject,
                UpdateType.Update, out var observer);

            return new OperatorMovementController(
                camera.SerializableComponents.transform,
                observer,
                operatorSerializableComponents.MovementConfig, _inputService);
        }
    }
}