using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push
{
    public class ExploderSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public ExplosionForceConfig ExplosionForceConfig { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float DelayBeforeExplosion { get; private set; } = 0.05f;
        [field: SerializeField] public bool ShouldExplodeWhenWasExplodedOutside { get; private set; }
    }
}