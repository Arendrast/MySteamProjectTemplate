using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic
{
    public class EffectableSerializableComponents : MonoBehaviour
    {
        [Serializable]
        public class ConfigByEffectTypePair
        {
            [field: SerializeReference] public IEffectReactorConfig EffectReactorConfig { get; private set; }
            [field: SerializeReference] public EffectType EffectType { get; private set; }
        }
        
        [field: SerializeField] public List<ConfigByEffectTypePair> EffectReactorsConfigs { get; private set; }
    }
}