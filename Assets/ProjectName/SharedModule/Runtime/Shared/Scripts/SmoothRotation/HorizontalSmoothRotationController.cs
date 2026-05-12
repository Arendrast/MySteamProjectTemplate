using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.SmoothRotation
{
    public class HorizontalSmoothRotationController
    {
        private readonly SmoothRotationConfig _config;

        public HorizontalSmoothRotationController(SmoothRotationConfig config)
        {
            _config = config;
        }

        public void Rotate(Vector3 direction)
        {
            var lookRotationY = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            
            _config.RotatableTransform.rotation = Quaternion.RotateTowards(
                _config.RotatableTransform.rotation,
                lookRotationY,
                _config.RotationSpeedInDegreesPerSecond * Time.deltaTime
            );
            
            if (!_config.RotationLimitationsConfig.Enabled)
            {
                return;
            }

            var yAngle = _config.RotatableTransform.localEulerAngles.y;
            if (yAngle > 180) yAngle -= 360; // Перевод в диапазон -180..180
            yAngle = Mathf.Clamp((float)yAngle, _config.RotationLimitationsConfig.MinimalDegrees,
                _config.RotationLimitationsConfig.MaximalDegrees);
            _config.RotatableTransform.localRotation = Quaternion.Euler(0, yAngle, 0);
        }
    }
}