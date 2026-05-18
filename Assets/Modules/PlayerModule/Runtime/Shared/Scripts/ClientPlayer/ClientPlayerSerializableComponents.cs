using FishNet.Object;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer
{
    public class ClientPlayerSerializableComponents: MonoBehaviour
    {
        [field: SerializeField] public NetworkObject NetworkObject { get; private set; }
        [field: SerializeField] public CapsuleCollider CharacterControllerCollider { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public ManyInvokableOneFrameCharacterController ManyInvokableOneFrameCharacterController { get; private set; }
        [field: SerializeField] public CapsuleOverlapObserver CapsuleOverlapObserver { get; private set; }
        [field: SerializeField] public EntitySerializableComponents EntityComponents { get; private set; }
        [field: SerializeField] public Transform ItemParentTransform { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
    }
}