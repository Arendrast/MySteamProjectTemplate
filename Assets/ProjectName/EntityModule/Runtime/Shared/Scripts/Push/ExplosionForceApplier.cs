using System.Collections.Generic;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push
{
    public class ExplosionForceApplier
    {
        public void Explode(IEnumerable<IPushable> pushables, Vector3 position, float forceAtCenter,
            float forceAtEdge,
            float radius, bool isBlockingExplosion, bool shouldMoveByYAxis)
        {
            foreach (var explodable in pushables)
            {
                explodable?.TryPush(position, forceAtCenter, forceAtEdge, radius, isBlockingExplosion, shouldMoveByYAxis);
            }
        }
    }
}