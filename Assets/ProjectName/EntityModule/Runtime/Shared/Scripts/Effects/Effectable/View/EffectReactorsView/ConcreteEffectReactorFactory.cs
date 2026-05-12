using System;
using Cysharp.Threading.Tasks;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View.EffectReactorsView
{
    public interface IConcreteEffectReactorViewFactory<TEffectReactorConfig, TEffectReactor> : IConcreteEffectReactorViewFactory
        where TEffectReactorConfig : IEffectReactorViewConfig
        where TEffectReactor : IEffectReactor
    {
        UniTask<IEffectReactorView> GetConcreteReactorAsync(TEffectReactorConfig config,
            EffectableViewSerializableComponents effectableSerializableComponents, bool isLocalPlayer, TEffectReactor reactor);
    }

    public interface IConcreteEffectReactorViewFactory : IMatchSharedFactory
    {
        UniTask<IEffectReactorView> GetReactorAsync(IEffectReactorViewConfig config,
            EffectableViewSerializableComponents effectableSerializableComponents, bool isOwner, IEffectReactor effectReactor);

        Type GetConfigType();
        Type GetLogicReactorType();
    }
}