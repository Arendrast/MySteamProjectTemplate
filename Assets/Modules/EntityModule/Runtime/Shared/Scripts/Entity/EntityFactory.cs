using System;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using FishNet.Object;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors;
using Modules.EntityModule.Runtime.Shared.Scripts.Heal;
using Modules.EntityModule.Runtime.Shared.Scripts.Repositories;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Entity
{
    public class EntityFactory : IMatchSharedFactory
    {
        public event Action<EntityComponents> CreatedEntity;

        private readonly HealthModelsRepository _healthModelsesIndexRepository;
        private readonly DamageReceiversRepository _damageReceiversModelIndexRepository;
        private readonly DamageDealersRepository _damageDealersModelIndexRepository;
        private readonly EntityComponentsRepository _entityComponentsRepository;
        private readonly ServerManager _serverManager;
        private readonly HealReceiversRepository _healReceiversesModelIndexRepository;
        private readonly HealDealersRepository _healDealersModelIndexRepository;
        private readonly EffectReceiversFactory _effectReceiversFactory;

        public EntityFactory(
            DamageReceiversRepository damageReceiversModelIndexRepository,
            DamageDealersRepository damageDealersModelIndexRepository,
            EntityComponentsRepository entityComponentsRepository,
            HealthModelsRepository healthModelsesIndexRepository,
            ServerManager serverManager, HealReceiversRepository healReceiversesModelIndexRepository,
            HealDealersRepository healDealersModelIndexRepository, EffectReceiversFactory effectReceiversFactory)
        {
            _damageReceiversModelIndexRepository = damageReceiversModelIndexRepository;
            _damageDealersModelIndexRepository = damageDealersModelIndexRepository;
            _entityComponentsRepository = entityComponentsRepository;
            _healthModelsesIndexRepository = healthModelsesIndexRepository;
            _serverManager = serverManager;
            _healReceiversesModelIndexRepository = healReceiversesModelIndexRepository;
            _healDealersModelIndexRepository = healDealersModelIndexRepository;
            _effectReceiversFactory = effectReceiversFactory;
        }

        public async UniTask<EntityComponents> GetCreatedEntityComponentsAsync(
            EntitySerializableComponents serializableComponents,
            bool shouldDisableOverlapObserver, 
            bool isOwner)
        {
            var networkObject = serializableComponents.GetComponentInParent<NetworkObject>(true);
            var networkObjectId = networkObject.ObjectId;

            var healthModel = _serverManager.Started
                ? new HealthModel(serializableComponents.MaxHealthPoints, name: serializableComponents.gameObject.name)
                : await _healthModelsesIndexRepository.GuaranteedGetValueByKeyAsync(networkObjectId);

            var damageReceiverModel = _serverManager.Started
                ? new DamageReceiverModel(networkObjectId, healthModel)
                : await _damageReceiversModelIndexRepository.GuaranteedGetValueByKeyAsync(networkObjectId);

            var healReceiverModel = _serverManager.Started
                ? new HealReceiverModel(healthModel)
                : await _healReceiversesModelIndexRepository.GuaranteedGetValueByKeyAsync(networkObjectId);

            var damageDealerModel = new DamageDealerModel(networkObjectId);

            var healDealerModel = new HealDealerModel(networkObjectId);

            if (_serverManager.Started)
            {
                _healthModelsesIndexRepository.Add(networkObjectId, healthModel);
                _damageReceiversModelIndexRepository.Add(networkObjectId, damageReceiverModel);
                _healReceiversesModelIndexRepository.Add(networkObjectId, healReceiverModel);
            }

            _damageDealersModelIndexRepository.Add(networkObjectId, damageDealerModel);
            _healDealersModelIndexRepository.Add(networkObjectId, healDealerModel);
            
            var components = new EntityComponents(
                damageDealerModel,
                damageReceiverModel,
                serializableComponents,
                healthModel,
                healReceiverModel, healDealerModel,
                await _effectReceiversFactory.GetEffectReceiverModel(serializableComponents
                    .EffectableSerializableComponents, isOwner));

            _entityComponentsRepository.Add(serializableComponents, components);

            ConfigureCapsuleOverlapObserver();

            serializableComponents.gameObject.GetOrAddComponent<DisableObserver>().Disabled +=
                RemoveComponentsFromRepositories;

            CreatedEntity?.Invoke(components);

            return components;

            void RemoveComponentsFromRepositories()
            {
                _entityComponentsRepository.RemoveByKey(serializableComponents);
                _damageReceiversModelIndexRepository.RemoveByKey(networkObjectId);
                _damageDealersModelIndexRepository.RemoveByKey(networkObjectId);
            }

            void ConfigureCapsuleOverlapObserver()
            {
                var capsuleOverlapObserver = serializableComponents.GetComponentInParent<CapsuleOverlapObserver>();

                if (shouldDisableOverlapObserver)
                {
                    capsuleOverlapObserver.enabled = false;
                }
            }
        }
    }
}