using System;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction;
using UnityEngine;

namespace Modules.LevelModule.Runtime.Shared.Scripts.DespawnTargetLevelZoneReaction
{
    [Serializable]
    public class DespawnTargetLevelZoneTriggerReactionConfig : IActionTriggerReactionConfig
    {
        [field: SerializeField] public float DelayBeforeReaction { get; private set; }
    }
}