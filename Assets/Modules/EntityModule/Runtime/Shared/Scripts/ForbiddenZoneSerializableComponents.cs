using Modules.OverlapModule.Runtime.Scripts;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts
{
    public class ForbiddenZoneSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public OverlapObserver OverlapObserver { get; private set; }
    }
}