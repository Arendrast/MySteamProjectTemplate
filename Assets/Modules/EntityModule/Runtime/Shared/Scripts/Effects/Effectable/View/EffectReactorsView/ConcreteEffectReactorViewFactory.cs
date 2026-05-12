using System;
using Cysharp.Threading.Tasks;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View.EffectReactorsView
{
    public abstract class ConcreteEffectReactorViewFactory<TEffectReactorViewConfig, TEffectReactor> 
        : IConcreteEffectReactorViewFactory<TEffectReactorViewConfig, TEffectReactor>
        where TEffectReactorViewConfig : IEffectReactorViewConfig
        where TEffectReactor : IEffectReactor
    {
        public abstract UniTask<IEffectReactorView> GetConcreteReactorAsync(TEffectReactorViewConfig config,
            EffectableViewSerializableComponents effectableSerializableComponents, bool isLocalPlayer, TEffectReactor effectReactor);

        public UniTask<IEffectReactorView> GetReactorAsync(IEffectReactorViewConfig config,
            EffectableViewSerializableComponents effectableSerializableComponents, bool isOwner, IEffectReactor effectReactor)
        {
            return GetConcreteReactorAsync((TEffectReactorViewConfig)config, effectableSerializableComponents, isOwner, (TEffectReactor) effectReactor);
        }

        public Type GetConfigType()
        {
            return typeof(TEffectReactorViewConfig);
        }
        
        public Type GetLogicReactorType()
        {
            return typeof(TEffectReactor);
        }
    }
}