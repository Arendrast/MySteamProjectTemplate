#if WWISE
using Cysharp.Threading.Tasks;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.PostEvent
{
    public class PostEventTriggerReactionFactory : ConcreteActionTriggerReactionFactory<
        PostEventTriggerReactionConfig>
    {
        public override UniTask<IActionTriggerReaction> GetConcreteReactionAsync(PostEventTriggerReactionConfig config)
        {
            return new UniTask<IActionTriggerReaction>(new PostEventTriggerReaction(config));
        }
    }
}
#endif