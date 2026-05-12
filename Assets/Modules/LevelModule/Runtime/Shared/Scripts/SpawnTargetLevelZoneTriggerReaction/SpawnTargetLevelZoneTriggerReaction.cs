using Cysharp.Threading.Tasks;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction;

namespace Modules.LevelModule.Runtime.Shared.Scripts.SpawnTargetLevelZoneTriggerReaction
{
    public class SpawnTargetLevelZoneTriggerReaction : IActionTriggerReaction
    {
        private readonly SpawnTargetLevelZoneTriggerReactionConfig _reactionConfig;
        private readonly LevelZoneFactoryRepository _levelZoneFactoryRepository;

        public SpawnTargetLevelZoneTriggerReaction(
            SpawnTargetLevelZoneTriggerReactionConfig reactionConfig,
            LevelZoneFactoryRepository levelZoneFactoryRepository)
        {
            _reactionConfig = reactionConfig;
            _levelZoneFactoryRepository = levelZoneFactoryRepository;
        }

        public async void Invoke()
        {
            _levelZoneFactoryRepository.LevelZoneFactory
                .GetInitializedServerLevelZoneAsync(_reactionConfig.ZonePartNumber).Forget();
        }
    }
}