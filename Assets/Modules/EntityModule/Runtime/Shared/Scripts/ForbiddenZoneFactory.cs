using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.EntityModule.Runtime.Shared.Scripts.Repositories;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;

#if TWO_D
using ActualCollider = UnityEngine.Collider2D;
#else
using ActualCollider = UnityEngine.Collider;
#endif

namespace Modules.EntityModule.Runtime.Shared.Scripts
{
    public class ForbiddenZoneFactory : IMatchSharedFactory
    {
        private readonly EntityComponentsRepository _entityRepository;

        public ForbiddenZoneFactory(EntityComponentsRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public void InitializeForbiddenZone(ForbiddenZoneSerializableComponents forbiddenZoneSerializableComponents)
        {
            forbiddenZoneSerializableComponents.OverlapObserver.EventsProvider.Entered += OnEnterAndStay;
            forbiddenZoneSerializableComponents.OverlapObserver.EventsProvider.Stayed += OnEnterAndStay;
        }

        private void OnEnterAndStay(ActualCollider collider)
        {
            var entitySerializableComponents =
                collider.transform.root?.GetComponentInChildren<EntitySerializableComponents>();

            if (entitySerializableComponents != null &&
                _entityRepository.TryGetValue(entitySerializableComponents, out var entityComponents) &&
                !entityComponents.HealthModel.IsDied)
            {
                entityComponents.DamageDealerModel.DoDamage(entityComponents.DamageReceiverModel,
                    new DoDamageData(entityComponents.HealthModel.HealthPoints, DamageOrigin.ForbiddenZone));
            }
        }
    }
}