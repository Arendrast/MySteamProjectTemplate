using System;
using UnityEngine;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters
{
    [Serializable]
    public class UpdateCounterTriggerReactionConfig : IActionTriggerReactionConfig
    {
        [field: SerializeField] public float DelayBeforeReaction { get; private set; }
        [field: SerializeField] public CounterType CounterType { get; private set; }
        [field: SerializeField] public int Value { get; private set; }
        [field: SerializeField] public bool CanUpdateIfValueIsLessOrEqual { get; private set; }
    }
}