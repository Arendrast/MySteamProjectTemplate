using System.Collections.Generic;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Configs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic
{
    [ConfigScope(nameof(EffectableConfig))]
    [CreateAssetMenu(fileName = nameof(EffectableConfig),
        menuName = "Configs/" + nameof(EffectableConfig))]
    public class EffectableConfig : SerializedScriptableObject
    {
        [field: OdinSerialize] public IReadOnlyDictionary<EffectType, IEffectReactorConfig> EffectReactorConfigsByEffectType
        {
            get;
            private set;
        }
    }
}