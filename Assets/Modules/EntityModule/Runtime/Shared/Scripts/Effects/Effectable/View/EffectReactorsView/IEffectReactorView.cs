using System.Collections.Generic;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View.EffectReactorsView
{
    public interface IEffectReactorView
    {
        void OnApply(IReadOnlyCollection<EffectType> otherActiveEffectTypes);
        void OnUpdate();
        void OnCancel(IReadOnlyCollection<EffectType> otherActiveEffectTypes);
    }
}