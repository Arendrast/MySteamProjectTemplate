namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.SetActiveGameObjects
{
    public class SetActiveGameObjectsTriggerReaction : IActionTriggerReaction
    {
        private readonly SetActiveGameObjectsTriggerReactionConfig _config;
        
        public SetActiveGameObjectsTriggerReaction(SetActiveGameObjectsTriggerReactionConfig config)
        {
            _config = config;
        }
        
        public void Invoke()
        {
            _config.GameObjects.ForEach(gameObject => gameObject.SetActive(_config.ShouldBeActive));
        }
    }
}