using System.Collections.Generic;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate;
using UnityEngine;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactor
{
    public class ActionTriggerReactorSerializableComponents : MonoBehaviour
    {
        [field: SerializeReference] public List<IActionTriggerConfig> TriggerConfigs { get; private set; } = new List<IActionTriggerConfig>();
        [field: SerializeReference] public List<IActionTriggerReactionConfig> TriggerReactionConfigs { get; private set; } = new List<IActionTriggerReactionConfig>();
        [field: SerializeField] public bool ShouldDisposeAfterReaction { get; private set; } = true;
        [field: SerializeField] public bool ServerAuthoritative { get; private set; }
    }
}