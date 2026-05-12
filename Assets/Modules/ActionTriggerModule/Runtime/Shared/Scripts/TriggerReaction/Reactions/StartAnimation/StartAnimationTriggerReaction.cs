namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.StartAnimation
{
    public class StartAnimationTriggerReaction : IActionTriggerReaction
    {
        private readonly StartAnimationTriggerReactionConfig _config;
        
        public StartAnimationTriggerReaction(StartAnimationTriggerReactionConfig config)
        {
            _config = config;
        }
        
        public void Invoke()
        {
            _config.AnimancerComponent.Play(_config.Clip);
        }
    }
}