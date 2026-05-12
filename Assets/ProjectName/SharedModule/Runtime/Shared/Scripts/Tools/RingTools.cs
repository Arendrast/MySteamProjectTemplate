using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class RingTools
    {
        public static Vector3 GetPointAroundCenter(
            Vector3 center,
            float angle,
            float radius,
            Vector3 direction,
            Vector3 rotationAxis)
        {
            var rotation = Quaternion.AngleAxis(angle, rotationAxis);
            var rotatedDirection = rotation * direction.normalized;
            var pointOnCircle = rotatedDirection * radius;
            return center + pointOnCircle;
        }
    }
}