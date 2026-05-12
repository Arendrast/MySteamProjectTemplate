using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Entity
{
    public class EntitySerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public EffectableSerializableComponents EffectableSerializableComponents { get; private set;}
        [field: SerializeField] public int MaxHealthPoints { get; private set; } = 100;
    }
}