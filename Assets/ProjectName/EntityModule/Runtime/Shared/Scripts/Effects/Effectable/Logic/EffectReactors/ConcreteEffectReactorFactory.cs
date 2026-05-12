using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors
{
    public abstract class ConcreteEffectReactorFactory<TEffectReactorConfig> : IConcreteEffectReactorFactory<TEffectReactorConfig>
        where TEffectReactorConfig : IEffectReactorConfig
    {
        public abstract UniTask<IEffectReactor> GetConcreteReactorAsync(TEffectReactorConfig config,
            EffectableSerializableComponents effectableSerializableComponents, bool isOwner);

        public abstract IReadOnlyList<EffectType> GetEffectTypes();

        public UniTask<IEffectReactor> GetReactorAsync(IEffectReactorConfig config,
            EffectableSerializableComponents effectableSerializableComponents, bool isOwner)
        {
            return GetConcreteReactorAsync((TEffectReactorConfig)config, effectableSerializableComponents, isOwner);
        }

        public Type GetConfigType()
        {
            return typeof(TEffectReactorConfig);
        }
    }
}