using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters
{
    public class NetworkCountersSynchronizerBehaviourRepository : IMatchSharedService
    {
        public NetworkCountersSynchronizerBehaviour Behaviour { get; set; }
    }
}