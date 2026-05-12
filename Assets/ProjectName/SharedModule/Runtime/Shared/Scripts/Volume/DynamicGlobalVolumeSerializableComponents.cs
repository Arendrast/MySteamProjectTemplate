using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Volume
{
    public class DynamicGlobalVolumeSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public UnityEngine.Rendering.Volume MainVolume { get; private set; }
        [field: SerializeField] public UnityEngine.Rendering.Volume BurningVolume { get; private set; }
    }
}