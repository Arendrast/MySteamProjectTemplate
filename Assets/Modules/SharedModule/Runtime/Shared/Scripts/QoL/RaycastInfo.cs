using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public readonly struct RaycastInfo
    {
        public readonly RaycastHit Hit;
        public readonly Vector3 Origin;
        public readonly Vector3 Direction;

        public RaycastInfo(RaycastHit hit, Vector3 origin, Vector3 direction)
        {
            Hit = hit;
            Origin = origin;
            Direction = direction.normalized;
        }
    }
}