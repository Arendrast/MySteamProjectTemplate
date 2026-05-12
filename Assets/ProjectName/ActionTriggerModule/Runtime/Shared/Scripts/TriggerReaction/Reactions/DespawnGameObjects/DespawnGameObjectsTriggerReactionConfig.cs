using System;
using UnityEngine;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.DespawnGameObjects
{
    [Serializable]
    public class DespawnGameObjectsTriggerReactionConfig : IActionTriggerReactionConfig
    {
        [field: SerializeField] public bool ShouldDespawnOnlyGameObjectsChildren { get; private set; }
        [field: SerializeField] public float DelayBeforeReaction { get; private set; }
        [field: SerializeField] public GameObject[] GameObjects { get; private set; }
    }
}