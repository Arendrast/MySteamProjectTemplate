using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.CameraPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Input;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Loading;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.Operator
{
    public class OperatorFactory : IMatchSharedFactory
    {
        private readonly IAssetLoader _assetLoader;
        private readonly CameraFactory _cameraFactory;
        private readonly IInputProvider _inputProvider;

        public OperatorFactory(IAssetLoader assetLoader, CameraFactory cameraFactory, IInputProvider inputProvider)
        {
            _assetLoader = assetLoader;
            _cameraFactory = cameraFactory;
            _inputProvider = inputProvider;
        }

        public async UniTask<OperatorMovementController> GetCreatedOperatorMovementController()
        {
            var operatorSerializableComponents =
                await AssetProvider.InstantiateAsync<OperatorSerializableComponents>("Operator", _assetLoader);

            var camera = await _cameraFactory.GetCreatedMainCameraAsync();
            
            operatorSerializableComponents.transform.SetParent(camera.SerializableComponents.Camera.transform);
            
            return new OperatorMovementController(
                camera.SerializableComponents.transform,
                operatorSerializableComponents.GetOrAddComponent<MonoBehaviourObserver>(),
                operatorSerializableComponents.MovementConfig, _inputProvider);
        }
    }
}