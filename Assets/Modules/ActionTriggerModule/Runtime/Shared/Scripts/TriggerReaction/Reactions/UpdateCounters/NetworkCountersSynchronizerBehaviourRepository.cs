using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters
{
    public class NetworkCountersSynchronizerBehaviourRepository : IMatchSharedService
    {
        public NetworkCountersSynchronizerBehaviour Behaviour { get; set; }
    }
}