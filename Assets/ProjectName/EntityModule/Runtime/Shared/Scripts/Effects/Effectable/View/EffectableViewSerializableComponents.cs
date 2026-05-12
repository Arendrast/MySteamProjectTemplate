using System.Collections.Generic;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View.EffectReactorsView;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View
{
    public class EffectableViewSerializableComponents : MonoBehaviour
    {
        [field: SerializeReference] public List<IEffectReactorViewConfig> EffectReactorsConfigs { get; private set; }
    }
}