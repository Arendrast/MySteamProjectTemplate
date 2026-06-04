#if TWO_D
using ActualCollider = UnityEngine.Collider2D;
#else
#endif
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts
{
    public class OverlapObserverModel
    {
        #if TWO_D
        public ValueModel<ContactFilter2D> ContactFilterModel { get; }
        #else
        public ValueModel<LayerMask> LayerMaskModel { get; } 
        public ValueModel<QueryTriggerInteraction> QueryTriggerInteractionModel { get; }
        #endif
        
        
        public ValueModel<bool> UpdateOverrideMaxOverlapsModel { get; }
        public ValueModel<int> MaxOverlapsModel { get; }
        public ValueModel<float> UpdateIntervalModel { get; }

        public OverlapObserverModel(OverlapObserverLogicConfig logicConfig)
        {
            #if TWO_D
            ContactFilterModel = new ValueModel<ContactFilter2D>(logicConfig.GetContactFilter);
            #else
            LayerMaskModel = new ValueModel<LayerMask>(logicConfig.GetLayerMask);
            QueryTriggerInteractionModel = new ValueModel<QueryTriggerInteraction>(logicConfig.GetQueryTriggerInteraction);
            #endif
            
            UpdateOverrideMaxOverlapsModel = new ValueModel<bool>(logicConfig.GetOverrideMaxOverlaps);
            MaxOverlapsModel = new ValueModel<int>(logicConfig.GetMaxOverlaps);
            UpdateIntervalModel = new ValueModel<float>(logicConfig.GetUpdateInterval);
        }
    }
}