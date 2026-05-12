using Cysharp.Threading.Tasks;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts.SpawnTargetLevelZoneTriggerReaction
{
    public class SpawnTargetLevelZoneTriggerReactionFactory : ConcreteActionTriggerReactionFactory<
        SpawnTargetLevelZoneTriggerReactionConfig>
    {
        private readonly LevelZoneFactoryRepository _levelZoneFactoryRepository;

        public SpawnTargetLevelZoneTriggerReactionFactory(
            LevelZoneFactoryRepository levelZoneFactoryRepository)
        {
            _levelZoneFactoryRepository = levelZoneFactoryRepository;
        }

        public override UniTask<IActionTriggerReaction> GetConcreteReactionAsync(
            SpawnTargetLevelZoneTriggerReactionConfig config)
        {
            return new UniTask<IActionTriggerReaction>(
                new SpawnTargetLevelZoneTriggerReaction(config, _levelZoneFactoryRepository));
        }
    }
}