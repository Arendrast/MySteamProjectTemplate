using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class FPSCameraSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public MonoBehaviourObserver MonoBehaviourObserver { get; private set; }
    }
}