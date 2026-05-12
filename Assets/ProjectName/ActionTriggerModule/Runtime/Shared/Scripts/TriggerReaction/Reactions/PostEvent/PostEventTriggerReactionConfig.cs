#if WWISE
using System;
using CrazySWAT.SharedModule.Runtime.Shared.Scripts;
using CrazySWAT.SharedModule.Runtime.Shared.Scripts.Sounds;
using UnityEngine;

namespace CrazySWAT.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.PostEvent
{
    [Serializable]
    public class PostEventTriggerReactionConfig : IActionTriggerReactionConfig
    {
        [field: SerializeField] public PostEventPlayer PostEventPlayer { get; private set; }
        [field: SerializeField] public string EventName { get; private set; }
        [field: SerializeField] public float DelayBeforeReaction { get; private set; }
    }
}
#endif