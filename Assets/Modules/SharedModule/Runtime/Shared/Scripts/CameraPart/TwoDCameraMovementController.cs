using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class TwoDCameraMovementController
    {
        private Transform _target;
        private Vector3 _currentVelocity;

        private readonly TwoDCameraSerializableComponents _serializableComponents;
        private readonly IInputService _inputService;
        private readonly CameraControllerData _cameraControllerData;

        private readonly Transform _cameraTransform;
        private readonly UpdateObserver _movementCameraUpdateObserver;

        public TwoDCameraMovementController(TwoDCameraSerializableComponents serializableComponents,
            IInputService inputService,
            CameraControllerData cameraControllerData, Transform moveCameraTransform,
            UpdateObserver movementCameraUpdateObserver)
        {
            _serializableComponents = serializableComponents;
            _inputService = inputService;
            _cameraControllerData = cameraControllerData;
            _cameraTransform = moveCameraTransform;
            _movementCameraUpdateObserver = movementCameraUpdateObserver;
        }

        public void SetPosition(Vector3 position)
        {
            _cameraTransform.position = position;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void StartMoveToTarget()
        {
            StopMoveToTarget();
            _movementCameraUpdateObserver.Updated += MoveToTargetPosition;
        }

        public void StopMoveToTarget()
        {
            _movementCameraUpdateObserver.Updated -= MoveToTargetPosition;
        }

        private void MoveToTargetPosition(float deltaTime)
        {
            var wishDirection = GetTargetPosition() - _cameraTransform.position;

            MovementTools.MoveTowards(wishDirection, _serializableComponents.MovementConfig.MaxSpeed, _serializableComponents.MovementConfig.AccelerationPerSecond, deltaTime,
                _cameraTransform, ref _currentVelocity);
            
            
            _cameraTransform.transform.position = GetTargetPosition();
        }

        private Vector3 GetTargetPosition()
        {
            return _target.position +
                   new Vector3(_inputService.MoveAction.x == 0
                           ? Mathf.Sign(_inputService.MoveAction.x) * _serializableComponents.MovementConfig.MaxDeltaOffset
                           : 0,
                       0, 0);
        }
    }
}