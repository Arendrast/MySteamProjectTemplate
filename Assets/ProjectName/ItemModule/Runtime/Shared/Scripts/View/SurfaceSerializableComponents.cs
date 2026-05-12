using UnityEngine;

namespace ProjectName.ItemModule.Runtime.Shared.Scripts.View
{
    public class SurfaceSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public SurfaceType SurfaceType { get; private set; }
        [field: SerializeField] public bool ShouldSpawnDecal { get; private set; } = true;
    }
}