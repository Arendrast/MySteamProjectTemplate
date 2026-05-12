using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push
{
    public class ExplodableSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public float LocalMass { get; private set; }
    }
}