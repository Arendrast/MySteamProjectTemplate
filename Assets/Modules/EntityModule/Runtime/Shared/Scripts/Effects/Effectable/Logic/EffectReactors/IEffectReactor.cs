using System;
using System.Collections.Generic;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors
{
    public interface IEffectReactor
    {
        event Action<IReadOnlyCollection<EffectType>> Applied, Cancelled;
        event Action Updated;
        
        void OnApply(IReadOnlyCollection<EffectType> otherActiveEffectTypes, int effectApplierId);
        void OnUpdate();
        void OnCancel(IReadOnlyCollection<EffectType> otherActiveEffectTypes);
    }
}