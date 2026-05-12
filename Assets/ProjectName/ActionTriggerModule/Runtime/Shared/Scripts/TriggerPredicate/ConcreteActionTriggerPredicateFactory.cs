using System;
using Cysharp.Threading.Tasks;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate
{
    public abstract class ConcreteActionTriggerPredicateFactory<TActionTriggerConfig> :
        IConcreteActionTriggerPredicateFactory<TActionTriggerConfig>
        where TActionTriggerConfig : IActionTriggerConfig
    {
        public abstract UniTask<IActionTriggerPredicate> GetConcretePredicateAsync(
            TActionTriggerConfig actionTriggerConfig);

        public UniTask<IActionTriggerPredicate> GetPredicateAsync(IActionTriggerConfig actionTriggerConfig)
        {
            return GetConcretePredicateAsync((TActionTriggerConfig)actionTriggerConfig);
        }

        public Type GetConfigType()
        {
            return typeof(TActionTriggerConfig);
        }
    }
}