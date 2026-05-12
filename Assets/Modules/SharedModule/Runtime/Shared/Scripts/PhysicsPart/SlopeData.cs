using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart
{
    public readonly struct SlopeData
    {
        public readonly Vector3 SlideDirection;
        public readonly Vector3 SlopeNormal;
        public readonly float SlopeAngle;

        public SlopeData(Vector3 slideDirection, Vector3 slopeNormal, float slopeAngle)
        {
            SlideDirection = slideDirection;
            SlopeNormal = slopeNormal;
            SlopeAngle = slopeAngle;
        }
    }
}