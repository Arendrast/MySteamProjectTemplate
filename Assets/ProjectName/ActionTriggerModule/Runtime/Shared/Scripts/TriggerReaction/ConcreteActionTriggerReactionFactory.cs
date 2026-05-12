using System;
using Cysharp.Threading.Tasks;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction
{
    public abstract class ConcreteActionTriggerReactionFactory<TActionTriggerReactionConfig> : IConcreteActionTriggerReactionFactory
        where TActionTriggerReactionConfig : IActionTriggerReactionConfig
    {
        public abstract UniTask<IActionTriggerReaction> GetConcreteReactionAsync(
            TActionTriggerReactionConfig config);

        public UniTask<IActionTriggerReaction> GetReactionAsync(IActionTriggerReactionConfig actionTriggerConfig)
        {
            return GetConcreteReactionAsync((TActionTriggerReactionConfig)actionTriggerConfig);
        }

        public Type GetConfigType()
        {
            return typeof(TActionTriggerReactionConfig);
        }
    }
}