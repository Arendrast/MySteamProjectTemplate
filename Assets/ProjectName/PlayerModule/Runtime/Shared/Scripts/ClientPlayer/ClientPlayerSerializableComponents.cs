using FishNet.Object;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using ProjectName.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using UnityEngine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer
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
        [field: SerializeField] public MonoBehaviourObserver Observer { get; private set; }
    }
}