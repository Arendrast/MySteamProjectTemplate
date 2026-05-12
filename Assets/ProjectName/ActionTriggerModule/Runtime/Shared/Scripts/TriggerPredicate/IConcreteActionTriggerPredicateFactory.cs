using System;
using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate
{
    public interface IConcreteActionTriggerPredicateFactory<TActionTriggerConfig> : IConcreteActionTriggerPredicateFactory
        where TActionTriggerConfig : IActionTriggerConfig
    {
        UniTask<IActionTriggerPredicate> GetConcretePredicateAsync(TActionTriggerConfig actionTriggerConfig);
    }

    public interface IConcreteActionTriggerPredicateFactory : IMatchSharedFactory
    {
        UniTask<IActionTriggerPredicate> GetPredicateAsync(IActionTriggerConfig actionTriggerConfig);
        Type GetConfigType();
    }
}