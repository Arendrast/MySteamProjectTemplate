using Cysharp.Threading.Tasks;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate;
using Modules.EntityModule.Runtime.Shared.Scripts.Repositories;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.LevelModule.Runtime.Shared.Scripts.LevelZoneEnterStateTrigger
{
    public class LevelZoneEnterStateTriggerPredicateFactory : ConcreteActionTriggerPredicateFactory<
        LevelZoneEnterStateTriggerConfig>
    {
        private readonly ConfigsProviderService _configsProviderService;
        private readonly ClientsConnectionTrackingService _clientsConnectionTrackingService;
        private readonly ClientManager _clientManager;
        private readonly ServerManager _serverManager;
        private readonly EntityComponentsRepository _entityComponentsRepository;

        public LevelZoneEnterStateTriggerPredicateFactory(ConfigsProviderService configsProviderService,
            ClientsConnectionTrackingService clientsConnectionTrackingService, ClientManager clientManager,
            ServerManager serverManager, EntityComponentsRepository entityComponentsRepository)
        {
            _configsProviderService = configsProviderService;
            _clientsConnectionTrackingService = clientsConnectionTrackingService;
            _clientManager = clientManager;
            _serverManager = serverManager;
            _entityComponentsRepository = entityComponentsRepository;
        }

        public override async UniTask<IActionTriggerPredicate> GetConcretePredicateAsync(
            LevelZoneEnterStateTriggerConfig actionTriggerConfig)
        {
            return new LevelZoneEnterStateActionTriggerPredicate(
                actionTriggerConfig,
                (await _configsProviderService.GetConfigAsync<PhysicsLayersConfig>())[
                    PhysicsLayerGroup.PlayerCharacterController],
                _clientsConnectionTrackingService, _clientManager, _serverManager, _entityComponentsRepository);
        }
    }
}