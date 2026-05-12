using Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects
{
    public class EffectApplierSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public float LifeTime { get; private set; }
        [field: SerializeField] public float TimeBeforeCancelEffect { get; private set; }
        [field: SerializeField] public EffectType EffectType { get; private set; }
        [field: SerializeField] public OverlapObserver OverlapObserver { get; private set; }
    }
}