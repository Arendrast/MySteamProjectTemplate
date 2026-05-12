using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer
{
    public class OwnerPlayerSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public ClientPlayerSerializableComponents ClientSerializableComponents { get; private set; }
        [field: SerializeField] public Transform CameraFollow { get; private set; }
        [field: SerializeField] public float MaxInteractionDistance { get; private set; } = 2;
    }
}