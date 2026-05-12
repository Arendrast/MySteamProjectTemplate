using System;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction
{
    public interface IConcreteActionTriggerReactionFactory<TActionTriggerReactionConfig> : IConcreteActionTriggerReactionFactory
        where TActionTriggerReactionConfig : IActionTriggerReactionConfig
    {
        UniTask<IActionTriggerReaction> GetConcreteReactionAsync(TActionTriggerReactionConfig config);
    }

    public interface IConcreteActionTriggerReactionFactory : IMatchSharedFactory
    {
        UniTask<IActionTriggerReaction> GetReactionAsync(IActionTriggerReactionConfig config);
        Type GetConfigType();
    }
}