using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class PhysicsTools
    {
        public static RaycastHit GetNearestRaycastHitByPredicate(RaycastHit[] hits, int hitsCount,
            Predicate<RaycastHit> predicate, Vector3 raycastOrigin)
        {
            var nearestDistance = float.MaxValue;
            RaycastHit target = default;

            for (var i = 0; i < hitsCount; i++)
            {
                if (!predicate.Invoke(hits[i]))
                    continue;
                
                var vector = hits[i].point - raycastOrigin;
                
                var distance = vector.sqrMagnitude;

                if (nearestDistance > distance)
                {
                    nearestDistance = distance;
                    target = hits[i];
                }
            }

            return target;
        }

        public static Vector3 GetCollisionPoint(this Collider colA, Collider colB, out Vector3 closestPointOnB,
            out Vector3 closestPointOnA)
        {
            // Находим точку на поверхности B, ближайшую к центру A
            closestPointOnB = colB.ClosestPoint(colA.transform.position);

            // Находим точку на поверхности A, ближайшую к центру B
            closestPointOnA = colA.ClosestPoint(colB.transform.position);

            // Средняя точка между ними — это примерная точка контакта
            return (closestPointOnA + closestPointOnB) * 0.5f;
        }
    }
}