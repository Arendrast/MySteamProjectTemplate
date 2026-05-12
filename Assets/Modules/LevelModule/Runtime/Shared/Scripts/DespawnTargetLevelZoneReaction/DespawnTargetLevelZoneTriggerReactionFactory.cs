using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction;

namespace Modules.LevelModule.Runtime.Shared.Scripts.DespawnTargetLevelZoneReaction
{
    public class DespawnTargetLevelZoneTriggerReactionFactory : ConcreteActionTriggerReactionFactory<
        DespawnTargetLevelZoneTriggerReactionConfig>
    {
        private readonly ServerManager _serverManager;
        private readonly LevelZoneRepository _levelZoneRepository;

        public DespawnTargetLevelZoneTriggerReactionFactory(ServerManager serverManager,
            LevelZoneRepository levelZoneRepository)
        {
            _serverManager = serverManager;
            _levelZoneRepository = levelZoneRepository;
        }

        public override UniTask<IActionTriggerReaction> GetConcreteReactionAsync(
            DespawnTargetLevelZoneTriggerReactionConfig config)
        {
            return new UniTask<IActionTriggerReaction>(
                new DespawnTargetLevelZoneTriggerReaction(_levelZoneRepository, _serverManager));
        }
    }
}