using System;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction;
using UnityEngine;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts.DespawnTargetLevelZoneReaction
{
    [Serializable]
    public class DespawnTargetLevelZoneTriggerReactionConfig : IActionTriggerReactionConfig
    {
        [field: SerializeField] public float DelayBeforeReaction { get; private set; }
    }
}