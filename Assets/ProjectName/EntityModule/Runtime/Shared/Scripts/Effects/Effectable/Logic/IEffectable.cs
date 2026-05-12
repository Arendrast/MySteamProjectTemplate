using System;
using System.Collections.Generic;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic
{
    public interface IEffectable
    {
        int Id { get; }
        
        event Action<EffectType, int, EffectOrigin> AppliedEffect;
        event Action<EffectType, int> CancelledEffect;
        
        IReadOnlyCollection<EffectType> ActiveEffects { get; }
        void TryApplyEffect(EffectType effectType, int effectApplierId, EffectOrigin effectOrigin);
        bool TryCancelEffect(EffectType effectType, int effectCancellerId, float delay);
    }
}