using System;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.SpawnGameObjects
{
    [Serializable]
    public class SpawnGameObjectsTriggerReactionConfig : IActionTriggerReactionConfig
    {
        [field: SerializeField] public float DelayBeforeReaction { get; private set; }
        [field: SerializeField] public AssetReference[] AssetReferences { get; private set; }
        [field: SerializeField] public TransformConfig TransformConfig { get; private set; }
        [field: SerializeField] public bool ShouldNetworkSpawn { get; private set; }
    }
}