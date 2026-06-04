using FishNet.Object;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using UnityEngine;

#if TWO_D
using ActualMovementComponent = UnityEngine.Rigidbody2D;
using ActualCapsuleCollider = UnityEngine.CapsuleCollider2D;
using ActualCapsuleOverlapObserver = Modules.OverlapModule.Runtime.Scripts._2D.CapsuleOverlapObserver2D;
#else
using ActualMovementComponent = UnityEngine.CharacterController;
using ActualCapsuleCollider = UnityEngine.CapsuleCollider;
using ActualCapsuleOverlapObserver = Modules.OverlapModule.Runtime.Scripts._3D.CapsuleOverlapObserver;
#endif

namespace Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer
{
    public class ClientPlayerSerializableComponents: MonoBehaviour
    {
        [field: SerializeField] public NetworkObject NetworkObject { get; private set; }
        [field: SerializeField] public ActualCapsuleCollider CapsuleCollider { get; private set; }
        [field: SerializeField] public ActualMovementComponent MovementComponent { get; private set; }
        [field: SerializeField] public ActualCapsuleOverlapObserver CapsuleOverlapObserver { get; private set; }
        [field: SerializeField] public EntitySerializableComponents EntityComponents { get; private set; }
        [field: SerializeField] public Transform ItemParentTransform { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
    }
}