using Cysharp.Threading.Tasks;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.StartAnimation
{
    public class StartAnimationTriggerReactionFactory : ConcreteActionTriggerReactionFactory<
        StartAnimationTriggerReactionConfig>
    {
        public override UniTask<IActionTriggerReaction> GetConcreteReactionAsync(StartAnimationTriggerReactionConfig config)
        {
            return new UniTask<IActionTriggerReaction>(new StartAnimationTriggerReaction(config));
        }
    }
}