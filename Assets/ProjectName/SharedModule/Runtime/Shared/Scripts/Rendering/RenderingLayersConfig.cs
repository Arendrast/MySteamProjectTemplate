using System.Collections.Generic;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Configs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Rendering
{
    [ConfigScope(nameof(RenderingLayersConfig))]
    [CreateAssetMenu(fileName = nameof(RenderingLayersConfig), menuName = "Configs/" + nameof(RenderingLayersConfig))]
    public class RenderingLayersConfig : SerializedScriptableObject
    {
        public RenderingLayerMask this[RenderingLayerGroup group] => LayerMaskByLayerGroup[group];

        [field: OdinSerialize]
        [field: ReadOnly]
        public Dictionary<RenderingLayerGroup, RenderingLayerMask> LayerMaskByLayerGroup { get; private set; }

        private void OnEnable()
        {
            LayerMaskByLayerGroup = new Dictionary<RenderingLayerGroup, RenderingLayerMask>()
            {
                { RenderingLayerGroup.RealtimeLight, RenderingLayerMask.GetMask("RealtimeLight") }
            };
        }
    }
}