using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts
{
    [Serializable]
    public class OverlapObserverLogicConfig
    {
        [field: Header("Настройки NonAlloc")]
        [field: SerializeField]
        public bool OverrideMaxOverlaps { get; private set; }

        [field: ShowIf(nameof(OverrideMaxOverlaps))]
        [field: SerializeField, Min(1)]
        public int MaxOverlaps { get; private set; } = 1000;

        [field: Space]
        [field: Header("Настройки Перекрытия")]

#if TWO_D
        [field: SerializeField]
        public ContactFilter2D ContactFilter { get; private set; } = ContactFilter2D.noFilter;
#else
        [field: SerializeField]
        public LayerMask LayerMask { get; private set; } = UnityEngine.Physics.DefaultRaycastLayers;

        [field: SerializeField]
        public QueryTriggerInteraction QueryTriggerInteraction { get; private set; } =
            QueryTriggerInteraction.UseGlobal;
#endif

        [field: SerializeField]
        [field: Range(0.001f, 5f)]
        public float UpdateInterval { get; private set; } = 0.1f;

        public bool GetOverrideMaxOverlaps() => OverrideMaxOverlaps;
        public int GetMaxOverlaps() => MaxOverlaps;
#if TWO_D
        public ContactFilter2D GetContactFilter() => ContactFilter;
#else
        public QueryTriggerInteraction GetQueryTriggerInteraction() => QueryTriggerInteraction;
        public LayerMask GetLayerMask() => LayerMask;
#endif


        public float GetUpdateInterval() => UpdateInterval;
    }
}