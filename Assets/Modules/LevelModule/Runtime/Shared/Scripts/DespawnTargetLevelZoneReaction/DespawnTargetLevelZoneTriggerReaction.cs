using FishNet.Managing.Server;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.LevelModule.Runtime.Shared.Scripts.DespawnTargetLevelZoneReaction
{
    public class DespawnTargetLevelZoneTriggerReaction : IActionTriggerReaction
    {
        private readonly LevelZoneRepository _levelZoneRepository;
        private readonly ServerManager _serverManager;

        public DespawnTargetLevelZoneTriggerReaction(LevelZoneRepository levelZoneRepository,
            ServerManager serverManager)
        {
            _serverManager = serverManager;
            _levelZoneRepository = levelZoneRepository;
        }

        public void Invoke()
        {
            _serverManager.TryDespawnOrDestroyAsync(_levelZoneRepository.TargetLevelZoneSerializableComponents
                .gameObject);
        }
    }
}