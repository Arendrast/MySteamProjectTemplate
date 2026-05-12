using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public class CurvedSpeedMovementController
    {
        private Vector3 _startPoint;
        private Vector3 _targetEndPoint;
        private readonly CurvedSpeedMovementCalculator _calculator;
        private readonly Transform _transform;

        public CurvedSpeedMovementController(CurvedSpeedMovementCalculator calculator, Transform transform)
        {
            _calculator = calculator;
            _transform = transform;
        }

        public void Configure(Vector3 startPoint,
            Vector3 targetEndPoint,
            AnimationCurve speedCurve, float accelerationTime,
            float maxSpeed)
        {
            _calculator.Configure(startPoint, targetEndPoint, speedCurve, accelerationTime, maxSpeed);
            _startPoint = startPoint;
            _targetEndPoint = targetEndPoint;
        }

        public void TryUpdateMovement(out bool didEndMove)
        {
            _calculator.TryUpdateMovement(out didEndMove, out var distanceToMoveThisFrame, out var pathProgress);

            if (didEndMove)
                return;

            _transform.position = Vector3.Lerp(_startPoint, _targetEndPoint, pathProgress);
        }
    }
}