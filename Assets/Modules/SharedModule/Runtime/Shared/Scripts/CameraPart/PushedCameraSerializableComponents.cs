using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class PushedCameraSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody CameraRigidbody { get; private set; }
        [field: SerializeField] public Collider CameraCollider { get; private set; }
        [field: SerializeField] public GameObject SphereUnderCamera { get; private set; }
    }
}