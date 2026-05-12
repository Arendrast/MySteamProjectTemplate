using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.SmoothRotation
{
    public class VerticalSmoothRotationController
    {
        private float _currentXRotation;
        
        private readonly SmoothRotationConfig _config;
        
        public VerticalSmoothRotationController(SmoothRotationConfig config)
        {
            _config = config;
        }
        
        public void Rotate(Vector3 targetPosition)
        {
            var localTargetPos = _config.RotatableTransform.parent.InverseTransformPoint(targetPosition);
            
            var distanceXZ = new Vector2(localTargetPos.x, localTargetPos.z).magnitude;
            
            var targetXAngle = -Mathf.Atan2(localTargetPos.y, distanceXZ) * Mathf.Rad2Deg;

            if (_config.RotationLimitationsConfig.Enabled)
            {
                targetXAngle = Mathf.Clamp(targetXAngle,
                    _config.RotationLimitationsConfig.MinimalDegrees,
                    _config.RotationLimitationsConfig.MaximalDegrees);
            }

            _currentXRotation = Mathf.MoveTowards(_currentXRotation, targetXAngle,
                _config.RotationSpeedInDegreesPerSecond * Time.deltaTime);
            
            var currentY = _config.RotatableTransform.localEulerAngles.y;
    
            _config.RotatableTransform.localRotation = Quaternion.Euler(_currentXRotation, currentY, 0);
        }
    }
}