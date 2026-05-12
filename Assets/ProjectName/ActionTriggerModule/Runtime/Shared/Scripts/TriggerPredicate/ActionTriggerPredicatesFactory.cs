using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate
{
    public class ActionTriggerPredicatesFactory : IMatchSharedFactory
    {
        private readonly Dictionary<Type, IConcreteActionTriggerPredicateFactory> _concretePredicatesFactories;

        public ActionTriggerPredicatesFactory(IEnumerable<IConcreteActionTriggerPredicateFactory> factories)
        {
            _concretePredicatesFactories = factories.ToDictionary(factory => factory.GetConfigType(), factory => factory);
        }

        public async UniTask<IActionTriggerPredicate> GetCreatedActionTriggerPredicateAsync(IActionTriggerConfig actionTriggerConfig)
        {
            if (!_concretePredicatesFactories.TryGetValue(actionTriggerConfig.GetType(),
                    out var concreteFactory))
            {
                return null;
            }

            return await concreteFactory.GetPredicateAsync(actionTriggerConfig);
        }
    }
}