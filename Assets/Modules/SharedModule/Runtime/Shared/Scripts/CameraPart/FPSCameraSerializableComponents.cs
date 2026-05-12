using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class FPSCameraSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public MonoBehaviourObserver MonoBehaviourObserver { get; private set; }
    }
}