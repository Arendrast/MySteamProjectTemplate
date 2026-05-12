using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts
{
    public class LevelZoneSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public bool IsPersistent { get; private set; }
        [field: SerializeField] public Transform[] SpawnersPositions { get; private set; }
        [field: SerializeField] public bool UseLightingConfig { get; private set; } = true;
        [field: ShowIf(nameof(UseLightingConfig))]
        [field: SerializeField] public LightingConfig LightingConfig { get; private set; }
    }
}