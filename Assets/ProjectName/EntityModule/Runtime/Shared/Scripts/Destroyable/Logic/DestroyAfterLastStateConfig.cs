using System;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Destroyable.Logic
{
    [Serializable]
    public class DestroyAfterLastStateConfig
    {
        [field: SerializeField] public float TimeBeforeDestroyAfterReachState { get; private set; }
        [field: SerializeField] public bool ShouldDestroy { get; private set; }
    }
}