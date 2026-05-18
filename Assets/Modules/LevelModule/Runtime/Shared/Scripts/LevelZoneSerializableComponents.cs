using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.LevelModule.Runtime.Shared.Scripts
{
    [RequireComponent(typeof(ChildrenSerializableComponentsContainer))]
    public class LevelZoneSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public bool IsPersistent { get; private set; }
        [field: SerializeField] public ChildrenSerializableComponentsContainer ChildrenSerializableComponentsContainer { get; private set; }
        [field: SerializeField] public Transform[] SpawnersPositions { get; private set; }
        [field: SerializeField] public bool UseLightingConfig { get; private set; } = true;
        [field: ShowIf(nameof(UseLightingConfig))]
        [field: SerializeField] public LightingConfig LightingConfig { get; private set; }

        private void OnValidate()
        {
            ChildrenSerializableComponentsContainer = gameObject.GetComponent<ChildrenSerializableComponentsContainer>();
        }
    }
}