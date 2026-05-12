using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction
{
    public class ActionTriggerReactionsFactory : IMatchSharedFactory
    {
        private readonly Dictionary<Type, IConcreteActionTriggerReactionFactory> _concretePredicatesFactories;

        public ActionTriggerReactionsFactory(IEnumerable<IConcreteActionTriggerReactionFactory> factories)
        {
            _concretePredicatesFactories = factories.ToDictionary(factory => factory.GetConfigType(), factory => factory);
        }

        public async UniTask<IActionTriggerReaction> GetCreatedActionTriggerReactionAsync(IActionTriggerReactionConfig config)
        {
            if (!_concretePredicatesFactories.TryGetValue(config.GetType(),
                    out var concreteFactory))
            {
                return null;
            }

            return await concreteFactory.GetReactionAsync(config);
        }
    }
}