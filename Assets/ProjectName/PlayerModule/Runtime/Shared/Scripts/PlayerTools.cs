using System;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts
{
    public static class PlayerTools
    {
        public static RaycastHit GetNearestRaycastHitExceptOwner(RaycastHit[] hits, int hitsCount,
            Transform playerTransform, Vector3 raycastOrigin,
            Predicate<RaycastHit> additionalCondition = null)
        {
            return PhysicsTools.GetNearestRaycastHitByPredicate(hits, hitsCount,
                hit => hit.collider.transform.root != playerTransform &&
                       (additionalCondition == null || additionalCondition.Invoke(hit)), raycastOrigin);
        }
    }
}