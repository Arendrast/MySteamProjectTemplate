using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts
{
    public class ForbiddenZoneSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public OverlapObserver OverlapObserver { get; private set; }
    }
}