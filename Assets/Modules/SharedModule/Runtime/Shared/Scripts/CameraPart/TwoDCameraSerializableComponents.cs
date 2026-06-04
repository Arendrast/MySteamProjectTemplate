using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class TwoDCameraSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public TwoDCameraMovementConfig MovementConfig { get; private set; }
    }
}