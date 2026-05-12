using System;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction;
using UnityEngine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.PostEventReaction
{
    [Serializable]
    public class PostEventOnLocalPlayerTriggerReactionConfig : IActionTriggerReactionConfig
    {
        [field: SerializeField] public float DelayBeforeReaction { get; private set; }
        [field: SerializeField] public string EventName { get; private set; }
    }
}