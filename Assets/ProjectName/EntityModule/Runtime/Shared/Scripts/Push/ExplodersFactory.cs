using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Object;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Repositories;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Configs;
using ProjectName.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push
{
    public class ExplodersFactory : IMatchSharedFactory
    {
        private readonly PushablesRepository _explodablesesRepository;
        private readonly DamageReceiversRepository _damageReceiversesRepository;

        private readonly HealthModelsRepository _healthModelsesRepository;

        private readonly DamageDealersRepository _damageDealersesRepository;
        private readonly ConfigsProviderService _configsProviderService;

        private readonly HashSet<ExploderSerializableComponents> _exploderSerializableComponentsSet =
            new HashSet<ExploderSerializableComponents>();

        public ExplodersFactory(
            PushablesRepository explodablesesRepository,
            DamageReceiversRepository damageReceiversesRepository,
            DamageDealersRepository damageDealersesRepository,
            HealthModelsRepository healthModelsesRepository,
            ConfigsProviderService configsProviderService)
        {
            _explodablesesRepository = explodablesesRepository;
            _damageReceiversesRepository = damageReceiversesRepository;
            _damageDealersesRepository = damageDealersesRepository;
            _healthModelsesRepository = healthModelsesRepository;
            _configsProviderService = configsProviderService;
        }

        public void TryCreateExploder(ExploderSerializableComponents exploderSerializableComponents)
        {
            if (!exploderSerializableComponents.TryGetComponent<NetworkObject>(out var networkObject) ||
                !_exploderSerializableComponentsSet.Add(exploderSerializableComponents))
            {
                return;
            }

            var healthModel = _healthModelsesRepository.ValueByKey.GetValueOrDefault(networkObject.ObjectId);

            if (healthModel == null)
                return;

            CreateExploderAsync(networkObject, exploderSerializableComponents,
                _damageReceiversesRepository.ValueByKey.GetValueOrDefault(networkObject.ObjectId) ??
                new DamageReceiverModel(networkObject.ObjectId, healthModel),
                _damageDealersesRepository.ValueByKey.GetValueOrDefault(networkObject.ObjectId) ??
                new DamageDealerModel(networkObject.ObjectId),
                healthModel);
        }

        private async void CreateExploderAsync(NetworkObject networkObject,
            ExploderSerializableComponents exploderSerializableComponents,
            DamageReceiverModel damageReceiverModel, DamageDealerModel damageDealerModel, HealthModel healthModel)
        {
            var explosionApplier = new DamagableExplosionApplyController(new ExplosionForceApplier(),
                new DamageReceiversFinder(_damageReceiversesRepository, damageDealerModel,
                    () => exploderSerializableComponents.transform.position),
                _explodablesesRepository, (await _configsProviderService.GetConfigAsync<PhysicsLayersConfig>())
                .LayerMaskByLayerGroup[
                    PhysicsLayerGroup.Environment]);

            _damageDealersesRepository.TryAdd(networkObject.ObjectId, damageDealerModel);
            _damageReceiversesRepository.TryAdd(networkObject.ObjectId, damageReceiverModel);
            _healthModelsesRepository.TryAdd(networkObject.ObjectId, healthModel);

            IPushable pushable = null;
            var wasExploded = false;

            healthModel.DiedWithoutArgs += TryExplodeAndDisposeAsyncWithoutArgs;

            if (exploderSerializableComponents.ShouldExplodeWhenWasExplodedOutside &&
                exploderSerializableComponents.TryGetComponent<ExplodableSerializableComponents>(
                    out var explodableSerializableComponents)
                && _explodablesesRepository.ValueByKey.TryGetValue(explodableSerializableComponents,
                    out pushable))
            {
                pushable.Pushed += TryExplodeAndDisposeAsync;
            }

            return;

            async void TryExplodeAndDisposeAsyncWithoutArgs()
            {
                TryExplodeAndDisposeAsync(true);
            }

            async void TryExplodeAndDisposeAsync(bool isBlockingExplosion)
            {
                if (wasExploded || !isBlockingExplosion)
                    return;

                wasExploded = true;

                await UniTask.WaitForSeconds(exploderSerializableComponents.DelayBeforeExplosion);

                if (!healthModel.IsDied)
                    damageDealerModel.DoDamage(damageReceiverModel, new DoDamageData(healthModel.HealthPoints, DamageOrigin.Explosion));

                healthModel.DiedWithoutArgs -= TryExplodeAndDisposeAsyncWithoutArgs;

                if (pushable != null)
                    pushable.Pushed -= TryExplodeAndDisposeAsync;

                explosionApplier.Explode(new ExplosionData(exploderSerializableComponents.transform.position,
                    exploderSerializableComponents.ExplosionForceConfig.ToData(),
                    exploderSerializableComponents.Damage,
                    (await _configsProviderService.GetConfigAsync<PhysicsLayersConfig>()).LayerMaskByLayerGroup[
                        PhysicsLayerGroup.Explodable],
                    exploderSerializableComponents.GetComponentsInChildren<Collider>()
                        .Select(transform => transform.gameObject).ToArray(), true));

                _healthModelsesRepository.RemoveByKey(networkObject.ObjectId);
                _damageDealersesRepository.RemoveByKey(networkObject.ObjectId);
                _damageReceiversesRepository.RemoveByKey(networkObject.ObjectId);
                _exploderSerializableComponentsSet.Remove(exploderSerializableComponents);
            }
        }
    }
}