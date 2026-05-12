#if WWISE
using CrazySWAT.SharedModule.Runtime.Shared.Scripts;

namespace CrazySWAT.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.PostEvent
{
    public class PostEventTriggerReaction : IActionTriggerReaction
    {
        private readonly PostEventTriggerReactionConfig _config;
        
        public PostEventTriggerReaction(PostEventTriggerReactionConfig config)
        {
            _config = config;
        }
        
        public void Invoke()
        {
            _config.PostEventPlayer.PostEvent(_config.EventName);
        }
    }
}
#endif