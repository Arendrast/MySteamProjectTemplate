using System.Collections.Generic;
using System.Linq;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Holders;
using UnityEngine;
using Gizmos = Popcron.Gizmos;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push
{
    public class DamagableExplosionApplyController
    {
        private readonly ExplosionForceApplier _explosionForceApplier;
        private readonly DamageReceiversFinder _damageReceiversFinder;

        private readonly PushablesRepository _explodablesesBySerializableComponents;
        private readonly LayerMask _environmentLayerMask;

        public DamagableExplosionApplyController(ExplosionForceApplier explosionForceApplier,
            DamageReceiversFinder damageReceiversFinder,
            PushablesRepository explodablesesBySerializableComponents, LayerMask environmentLayerMask)
        {
            _explosionForceApplier = explosionForceApplier;
            _damageReceiversFinder = damageReceiversFinder;
            _explodablesesBySerializableComponents = explodablesesBySerializableComponents;
            _environmentLayerMask = environmentLayerMask;
        }

        public void Explode(ExplosionData explosionData, bool shouldSortByDistance = true)
        {
            var colliders = Physics.OverlapSphere(explosionData.Position, explosionData.ExplosionForceData.Radius,
                explosionData.LayerMask);

            Gizmos.Sphere(explosionData.Position, explosionData.ExplosionForceData.Radius, Color.magenta,
                time: SharedConstantsHolder.GizmoDrawTime);

            if (colliders.Length == 0)
                return;

            if (shouldSortByDistance)
            {
                colliders = colliders.OrderBy(collider =>
                        Vector3.Distance(explosionData.Position, collider.transform.position))
                    .ToArray();
            }

            var resultColliders = new List<Collider>();
            var explodables = new List<IPushable>();

            var excludedGameObjectsIsNotEmpty = explosionData.ExcludedGameObjects is { Length: > 0 };

            foreach (var collider in colliders)
            {
                if (excludedGameObjectsIsNotEmpty &&
                    explosionData.ExcludedGameObjects.Contains(collider.gameObject))
                {
                    continue;
                }

                if (!collider.TryGetComponent<ExplodableSerializableComponents>(
                        out var explodableSerializableComponents) ||
                    !_explodablesesBySerializableComponents.ValueByKey.TryGetValue(
                        explodableSerializableComponents, out var explodable))
                {
                    continue;
                }
                
                if (DoesRaycastDetectWall(collider))
                {
                    continue;
                }

                explodables.Add(explodable);
                resultColliders.Add(collider);
            }

            _explosionForceApplier.Explode(explodables, explosionData.Position,
                explosionData.ExplosionForceData.ForceAtCenter,
                explosionData.ExplosionForceData.ForceAtEdge,
                explosionData.ExplosionForceData.Radius, true, explosionData.ShouldMoveByYAxis);

            _damageReceiversFinder.TryDoDamage(resultColliders, explosionData.Damage, DamageOrigin.Explosion);

            return;
            
            bool DoesRaycastDetectWall(Collider collider)
            {
                var closestPoint = collider.bounds.ClosestPoint(explosionData.Position);
                return Physics.Raycast(explosionData.Position,
                    (closestPoint - explosionData.Position).normalized,
                    Vector3.Distance(closestPoint, explosionData.Position), _environmentLayerMask,
                    QueryTriggerInteraction.Ignore);
            }
        }
    }
}