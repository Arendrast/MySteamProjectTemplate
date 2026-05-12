using System.Collections.Generic;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View.EffectReactorsView;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View
{
    public class EffectableViewSerializableComponents : MonoBehaviour
    {
        [field: SerializeReference] public List<IEffectReactorViewConfig> EffectReactorsConfigs { get; private set; }
    }
}