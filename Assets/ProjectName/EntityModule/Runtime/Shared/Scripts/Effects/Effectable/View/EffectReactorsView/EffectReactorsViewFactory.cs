using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View.EffectReactorsView
{
    public class EffectReactorsViewFactory : IMatchSharedFactory
    {
        private readonly Dictionary<KeyValuePair<Type, Type>, IConcreteEffectReactorViewFactory>
            _concreteReactorsFactory;

        public EffectReactorsViewFactory(IEnumerable<IConcreteEffectReactorViewFactory> factories)
        {
            _concreteReactorsFactory =
                factories.ToDictionary(
                    factory => KeyValuePair.Create(factory.GetConfigType(), factory.GetLogicReactorType()),
                    factory => factory);
        }

        public async UniTask<IEffectReactorView> GetCreatedEffectReactorAsync(IEffectReactor reactor,
            IEffectReactorViewConfig config, EffectableViewSerializableComponents effectableSerializableComponents,
            bool isLocalPlayer)
        {
            if (config == null || !_concreteReactorsFactory.TryGetValue(
                    KeyValuePair.Create(config.GetType(), reactor.GetType()),
                    out var concreteFactory))
            {
                return null;
            }

            var reactorView = await concreteFactory.GetReactorAsync(config, effectableSerializableComponents, isLocalPlayer, reactor);
            
            reactor.Applied += reactorView.OnApply;
            reactor.Updated += reactorView.OnUpdate;
            reactor.Cancelled += reactorView.OnCancel;

            return reactorView;
        }
    }
}