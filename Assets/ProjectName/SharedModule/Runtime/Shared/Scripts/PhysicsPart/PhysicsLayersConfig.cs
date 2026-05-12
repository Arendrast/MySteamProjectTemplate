using System.Collections.Generic;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Configs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.PhysicsPart
{
    [ConfigScope(nameof(PhysicsLayersConfig))]
    [CreateAssetMenu(fileName = nameof(PhysicsLayersConfig), menuName = "Configs/" + nameof(PhysicsLayersConfig))]
    public class PhysicsLayersConfig : SerializedScriptableObject
    {
        public LayerMask this[PhysicsLayerGroup group] => LayerMaskByLayerGroup[group];
        [field: OdinSerialize] public Dictionary<PhysicsLayerGroup, LayerMask> LayerMaskByLayerGroup { get; private set; }
    }
}