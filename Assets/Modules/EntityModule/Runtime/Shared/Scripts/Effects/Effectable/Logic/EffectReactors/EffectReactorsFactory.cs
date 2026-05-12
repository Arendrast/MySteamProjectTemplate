using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors
{
    public class EffectReactorsFactory : IMatchSharedFactory
    {
        private readonly Dictionary<KeyValuePair<Type, EffectType>, IConcreteEffectReactorFactory>
            _concreteReactorsFactory;

        public EffectReactorsFactory(IEnumerable<IConcreteEffectReactorFactory> factories)
        {
            _concreteReactorsFactory =
                factories.ToDictionary(
                        factory => KeyValuePair.Create(factory.GetConfigType(), factory.GetEffectTypes()),
                        factory => factory).SelectMany(pair =>
                        pair.Key.Value.Select(effectType => new
                        {
                            Key = new KeyValuePair<Type, EffectType>(pair.Key.Key, effectType),
                            Value = pair.Value
                        })
                    )
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public async UniTask<IEffectReactor> GetCreatedEffectReactorAsync(EffectType effectType,
            IEffectReactorConfig config, EffectableSerializableComponents effectableSerializableComponents,
            bool isOwner)
        {
            if (config == null || !_concreteReactorsFactory.TryGetValue(
                    KeyValuePair.Create(config.GetType(), effectType),
                    out var concreteFactory))
            {
                return null;
            }

            return await concreteFactory.GetReactorAsync(config, effectableSerializableComponents, isOwner);
        }
    }
}