using Modules.SharedModule.Runtime.Shared.Scripts.Animations;
using UnityEngine;

namespace Modules.ItemModule.Runtime.Shared.Scripts.View
{
    public class ItemViewSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public AnimationPlayer AnimationPlayer { get; private set; }
        [field: SerializeField] public Transform RightHandGripTarget { get; private set; }
        [field: SerializeField] public Transform LeftHandGripTarget { get; private set; }
        [field: SerializeField] public Vector3 LocalPosition { get; private set; }
        [field: SerializeField] public Vector3 LocalRotation { get; private set; }
        [field: SerializeField] public Transform UseEffectSpawnPoint { get; private set; }
    }
}