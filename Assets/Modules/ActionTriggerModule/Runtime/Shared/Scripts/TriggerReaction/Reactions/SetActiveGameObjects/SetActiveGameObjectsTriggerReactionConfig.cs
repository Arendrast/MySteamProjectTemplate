using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.SetActiveGameObjects
{
    [Serializable]
    public class SetActiveGameObjectsTriggerReactionConfig : IActionTriggerReactionConfig
    {
        [field: SerializeField] public List<GameObject> GameObjects { get; private set; } = new List<GameObject>();
        [field: SerializeField] public bool ShouldBeActive { get; private set; }
        [field: SerializeField] public float DelayBeforeReaction { get; private set; }
    }
}