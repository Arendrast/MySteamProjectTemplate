using System;
using Animancer;
using UnityEngine;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.StartAnimation
{
    [Serializable]
    public class StartAnimationTriggerReactionConfig : IActionTriggerReactionConfig
    {
        [field: SerializeField] public AnimancerComponent AnimancerComponent { get; private set; }
        [field: SerializeField] public AnimationClip Clip { get; private set; }
        [field: SerializeField] public float DelayBeforeReaction { get; private set; }
    }
}