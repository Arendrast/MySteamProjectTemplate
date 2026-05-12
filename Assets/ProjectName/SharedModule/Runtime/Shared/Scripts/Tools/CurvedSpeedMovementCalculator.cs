using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public class CurvedSpeedMovementCalculator
    {
        private float _speedProgress, _totalDistance, _pathProgress, _maxSpeed;
        private bool _isConfigured;
        private AnimationCurve _speedCurve;
        private float _accelerationTime;

        public void Configure(Vector3 startPoint,
            Vector3 targetEndPoint,
            AnimationCurve speedCurve, float accelerationTime,
            float maxSpeed)
        {
            _speedProgress = 0;
            _pathProgress = 0;
            _maxSpeed = maxSpeed;
            _speedCurve = speedCurve;
            _totalDistance = Vector3.Distance(startPoint, targetEndPoint);
            _accelerationTime = accelerationTime;
            _isConfigured = true;
        }

        public void TryUpdateMovement(out bool didEndMove, out float distanceToMoveThisFrame, out float pathProgress)
        {
            didEndMove = _pathProgress >= 1;
            pathProgress = 0;
            distanceToMoveThisFrame = 0;

            if (!_isConfigured || didEndMove)
                return;

            _speedProgress += Time.deltaTime / Mathf.Max(Time.deltaTime, _accelerationTime);
            _speedProgress = Mathf.Clamp01(_speedProgress);
            var curveValue = _speedCurve.Evaluate(_speedProgress);
            var currentSpeed = Mathf.Lerp(0, _maxSpeed, curveValue);
            distanceToMoveThisFrame = currentSpeed * Time.deltaTime;
            _pathProgress += distanceToMoveThisFrame / _totalDistance;
            _pathProgress = Mathf.Clamp01(_pathProgress);

            pathProgress = _pathProgress;
            didEndMove = _pathProgress >= 1;

            if (didEndMove)
                _isConfigured = false;
        }
    }
}