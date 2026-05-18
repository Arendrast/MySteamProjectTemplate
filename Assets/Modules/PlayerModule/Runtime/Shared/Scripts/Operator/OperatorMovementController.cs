using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
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
                
                var targetVelocity = wishDirection * config.MaxSpeed;
                
                _currentVelocity = Vector3.MoveTowards(
                    _currentVelocity, 
                    targetVelocity, 
                    config.AccelerationPerSecond * deltaTime
                );
                
                cameraTransform.transform.position += _currentVelocity * deltaTime;
            }
        }
    }
}