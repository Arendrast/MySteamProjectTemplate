using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Operator
{
    public class OperatorMovementController
    {
        private Vector3 _currentVelocity;

        public OperatorMovementController(Transform cameraTransform, MonoBehaviourObserver monoBehaviourObserver,
            OperatorMovementConfig config, IInputProvider inputProvider)
        {
            monoBehaviourObserver.Updated += Move;

            return;

            void Move()
            {
                var wishDirection = (cameraTransform.transform.forward * inputProvider.MoveAction.y) + 
                                        (cameraTransform.transform.right * inputProvider.MoveAction.x);
                
                var targetVelocity = wishDirection * config.MaxSpeed;
                
                _currentVelocity = Vector3.MoveTowards(
                    _currentVelocity, 
                    targetVelocity, 
                    config.AccelerationPerSecond * Time.deltaTime
                );
                
                cameraTransform.transform.position += _currentVelocity * Time.deltaTime;
            }
        }
    }
}