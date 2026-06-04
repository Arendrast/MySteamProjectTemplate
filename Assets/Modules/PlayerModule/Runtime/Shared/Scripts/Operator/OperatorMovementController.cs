using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Operator
{
    public class OperatorMovementController
    {
        private Vector3 _currentVelocity;

        public OperatorMovementController(Transform cameraTransform, UpdateObserver updateObserver,
            OperatorMovementConfig config, IInputService inputService)
        {
            updateObserver.Updated += Move;

            return;

            void Move(float deltaTime)
            {
                var wishDirection = (cameraTransform.transform.forward * inputService.MoveAction.y) +
                                    (cameraTransform.transform.right * inputService.MoveAction.x);

                MovementTools.MoveTowards(wishDirection, config.MaxSpeed, config.AccelerationPerSecond, deltaTime,
                    cameraTransform, ref _currentVelocity);
            }
        }
    }
}