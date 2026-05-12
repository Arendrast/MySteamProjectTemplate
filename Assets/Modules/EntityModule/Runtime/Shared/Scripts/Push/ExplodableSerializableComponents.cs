using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public class ExplodableSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public float LocalMass { get; private set; }
    }
}