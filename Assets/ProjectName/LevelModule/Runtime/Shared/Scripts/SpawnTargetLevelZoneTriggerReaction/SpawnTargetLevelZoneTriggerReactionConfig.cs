using System;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction;
using UnityEngine;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts.SpawnTargetLevelZoneTriggerReaction
{
    [Serializable]
    public class SpawnTargetLevelZoneTriggerReactionConfig : IActionTriggerReactionConfig
    {
        [field: SerializeField] public float DelayBeforeReaction { get; private set; }
        [field: SerializeField] public int ZonePartNumber { get; private set; }
    }
}