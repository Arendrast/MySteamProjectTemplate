using System.Linq;
using FishNet.Managing.Server;
using FishNet.Object;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.EntityModule.Runtime.Shared.Scripts.Repositories;
using Modules.SharedModule.Runtime.Shared.Scripts.Index;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Destroyable.Logic
{
    public class DestroyablesFactory : IMatchSharedFactory
    {
        private readonly DamageReceiversRepository _damageReceiversesRepository;
        private readonly HealthModelsRepository _healthModelsesRepository;
        private readonly DestroyablesRepository _destroyablesRepository;
        private readonly ServerManager _serverManager;

        public DestroyablesFactory(
            DamageReceiversRepository damageReceiversesRepository,
            HealthModelsRepository healthModelsesRepository,
            DestroyablesRepository destroyablesRepository,
            ServerManager serverManager)
        {
            _damageReceiversesRepository = damageReceiversesRepository;
            _healthModelsesRepository = healthModelsesRepository;
            _destroyablesRepository = destroyablesRepository;
            _serverManager = serverManager;
        }

        public void InitializeDestroyable(
            DestroyableSerializableComponents destroyableSerializableComponents,
            out HealthModel healthModel)
        {
            if (_destroyablesRepository.TryGetValue(destroyableSerializableComponents, out healthModel))
            {
                return;
            }

            if (destroyableSerializableComponents == null ||
                !destroyableSerializableComponents.DestructionStatesConfig.IsDestroyable)
            {
                return;
            }

            var networkObject = destroyableSerializableComponents.GetComponent<NetworkObject>();
            var networkObjectId = networkObject.ObjectId;

            if (!networkObject.IsSpawned)
            {
                return;
            }
            
            if (_serverManager.Started || !_healthModelsesRepository.ValueByKey.TryGetValue(networkObjectId, out healthModel))
            {
                var maxHp = destroyableSerializableComponents.DestructionStatesConfig.DestructionStateConfigs.Max(config => config.BorderHealthNumber);
                healthModel = new HealthModel(maxHp, name: destroyableSerializableComponents.name);
                
                if (!_serverManager.Started)
                    healthModel.TrySetHealthPoints(0, IndexableTools.MissingOrInvalidId);
            }
            
            if (_serverManager.Started)
            {
                var damageReceiverModel = new DamageReceiverModel(networkObjectId, healthModel);
                _healthModelsesRepository.Add(networkObjectId, healthModel);
                _damageReceiversesRepository.Add(networkObjectId, damageReceiverModel);
            }
            
            _destroyablesRepository.Add(destroyableSerializableComponents, healthModel);

            var destructionObserver = new DestructionObserver(healthModel,
                destroyableSerializableComponents.DestructionStatesConfig);

            var setActiveDestructionStateGameObjectController = new SetActiveDestructionStateGameObjectController(
                destroyableSerializableComponents.DestructionStatesConfig,
                destructionObserver.CurrentConfig);

            destructionObserver.ChangedDestructionState +=
                setActiveDestructionStateGameObjectController.SetActiveTrueOnlyActiveGameObject;

            if (!destroyableSerializableComponents.DestructionStatesConfig.DestroyConfig.ShouldDestroy)
            {
                return;
            }

            var destroyableAfterLastStateDestroyer = new DestroyableAfterDieDestroyer(
                destroyableSerializableComponents.DestructionStatesConfig,
                destroyableSerializableComponents.gameObject, _serverManager);

            healthModel.DiedWithoutArgs += destroyableAfterLastStateDestroyer.TryDespawnAfterTime;
        }
    }
}