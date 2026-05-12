using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors
{
    public interface IConcreteEffectReactorFactory<TEffectReactorConfig> : IConcreteEffectReactorFactory
        where TEffectReactorConfig : IEffectReactorConfig
    {
        UniTask<IEffectReactor> GetConcreteReactorAsync(TEffectReactorConfig config,
            EffectableSerializableComponents effectableSerializableComponents, bool isOwner);
    }

    public interface IConcreteEffectReactorFactory : IMatchSharedFactory
    {
        UniTask<IEffectReactor> GetReactorAsync(IEffectReactorConfig config,
            EffectableSerializableComponents effectableSerializableComponents, bool isOwner);

        Type GetConfigType();
        IReadOnlyList<EffectType> GetEffectTypes();
    }
}