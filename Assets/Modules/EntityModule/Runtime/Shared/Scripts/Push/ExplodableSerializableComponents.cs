using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public class ExplodableSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public PushableMovementType MovementType { get; set; }
        [field: SerializeField] public float LocalMass { get; private set; }
        [field: SerializeField] public bool ShouldDisableCapsuleOverlapObserverWhenIsInactive { get; private set; }
    }
}