using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
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
        [field: SerializeField]
        public LayerMask LayerMask { get; private set; } = UnityEngine.Physics.DefaultRaycastLayers;

        [field: SerializeField]
        public QueryTriggerInteraction QueryTriggerInteraction { get; private set; } = QueryTriggerInteraction.UseGlobal;

        [field: SerializeField]
        [field: Range(0.001f, 5f)]
        public float UpdateInterval { get; private set; } = 0.1f; 
        
        public void SetLayerMask(LayerMask layerMask)
        {
            LayerMask = layerMask;
        }

        public void SetQueryTriggerInteraction(QueryTriggerInteraction queryTriggerInteraction)
        {
            QueryTriggerInteraction = queryTriggerInteraction;
        }
    }
}