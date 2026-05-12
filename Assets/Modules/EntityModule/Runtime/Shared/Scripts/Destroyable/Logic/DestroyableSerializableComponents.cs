using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Destroyable.Logic
{
    [DisallowMultipleComponent]
    public class DestroyableSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public DestructionStatesConfig DestructionStatesConfig { get; private set; }
    }
}