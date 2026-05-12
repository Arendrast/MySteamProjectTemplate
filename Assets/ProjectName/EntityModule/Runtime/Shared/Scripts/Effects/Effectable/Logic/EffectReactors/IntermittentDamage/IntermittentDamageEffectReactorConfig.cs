using System;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors.IntermittentDamage
{
    [Serializable]
    public class IntermittentDamageReactorConfig
    {
        [field: SerializeField] public int DamagePerTick { get; private set; }
        [field: SerializeField] public int TicksPerSecond { get; private set; }
    }
}