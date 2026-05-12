using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Repositories;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts
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
            forbiddenZoneSerializableComponents.OverlapObserver.Entered += OnEnterAndStay;
            forbiddenZoneSerializableComponents.OverlapObserver.Stayed += OnEnterAndStay;
        }

        private void OnEnterAndStay(Collider collider)
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