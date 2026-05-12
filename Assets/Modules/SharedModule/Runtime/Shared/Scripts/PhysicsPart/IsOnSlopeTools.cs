using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart
{
    public static class IsOnSlopeTools
    {

        public static SlopeData GetSlopeData(this RaycastHit sphereHit, out bool isNull)
        {
            isNull = sphereHit.collider == null;
            
            if (isNull)
                return default;
            
            var finalNormal = sphereHit.normal;
            
            if (sphereHit.collider.Raycast(new Ray(sphereHit.point + Vector3.up * 0.1f, Vector3.down), out var rayHit, 1f)) 
            {
                finalNormal = rayHit.normal;
            }

            var angle = Vector3.Angle(Vector3.up, finalNormal);

            return new SlopeData(
                Vector3.ProjectOnPlane(Vector3.down, finalNormal).normalized,
                finalNormal,
                angle
            );
        }

        public static bool IsOnSlope(this RaycastHit hitUnderFeet, float slopeLimitAngle)
        {
            return IsOnSlope(hitUnderFeet, slopeLimitAngle, out var slopeData);
        }
        
        public static bool IsNotWall(this RaycastHit groundUnderFeet, out SlopeData slopeData)
        {
            return IsOnSlope(groundUnderFeet, 0, out slopeData);
        }

        public static bool IsOnSlope(this RaycastHit hitUnderFeet, float slopeLimitAngle, 
            out SlopeData slopeData)
        {
            slopeData = GetSlopeData(hitUnderFeet, out var isNull);
            
            return !isNull && slopeData.IsOnSlope(slopeLimitAngle);
        }
        
        public static bool IsOnSlope(this SlopeData slopeData, float slopeLimitAngle)
        {
            return slopeData.SlopeAngle >= slopeLimitAngle && slopeData.SlopeAngle < 89f;
        }
        
        public static SlopeData? GetSlopeData(this Collider ground, Vector3 startCheckPoint)
        {
            if (ground == null)
                return null;

            var closestPoint = ground.ClosestPoint(startCheckPoint);
            
            var slopeNormal = (startCheckPoint - closestPoint).normalized;

            if (slopeNormal == Vector3.zero) slopeNormal = Vector3.up; 

            var angle = Vector3.Angle(Vector3.up, slopeNormal);
           
            return new SlopeData(
                Vector3.ProjectOnPlane(Vector3.down, slopeNormal).normalized,
                slopeNormal,
                angle);
        }

        public static bool IsOnSlope(this Collider groundUnderFeet, Vector3 startCheckPoint, float slopeLimitAngle)
        {
            return IsOnSlope(groundUnderFeet, slopeLimitAngle, startCheckPoint, out var slopeData);
        }
        
        public static bool IsNotWall(this Collider groundUnderFeet, Vector3 startCheckPoint, out SlopeData slopeData)
        {
            return IsOnSlope(groundUnderFeet, 0, startCheckPoint, out slopeData);
        }

        public static bool IsOnSlope(this Collider groundUnderFeet, float slopeLimitAngle, Vector3 startCheckPoint,
            out SlopeData slopeData)
        {
            var localSlopeData = GetSlopeData(groundUnderFeet, startCheckPoint); 
            slopeData = localSlopeData ?? default;

            return localSlopeData != null && slopeData.IsOnSlope(slopeLimitAngle);
        }
    }
}