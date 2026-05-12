using System;
using System.Collections.Generic;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Destroyable.Logic
{
    [Serializable]
    public class DestructionStatesConfig
    {
        [Serializable]
        public class DestructionStateConfig
        {
            [field: SerializeField] public int BorderHealthNumber { get; private set; }
            [field: SerializeField] public GameObject ActiveGameObject { get; private set; }
            [field: SerializeField] public GameObject[] GameObjectsToUnlinkFromParentOnEnable { get; private set; }
            [field: SerializeField] public float TimeBeforeDestroyUnlinkedGameObjects { get; private set; }
        }
        
        [Serializable]
        public class DestroyAfterDieConfig
        {
            [field: SerializeField] public float TimeBeforeDestroyAfterDie { get; private set; }
            [field: SerializeField] public bool ShouldDestroy { get; private set; }
        }
        
        [field: SerializeField] public DestroyAfterDieConfig DestroyConfig { get; private set; }
        [field: SerializeField] public DamageReceiverConfig DamageReceiverConfig { get; private set; }
        [field: SerializeField] public bool IsDestroyable { get; private set; }
        [field: SerializeField] public List<DestructionStateConfig> DestructionStateConfigs { get; private set; }
    }
}