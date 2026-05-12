using Cysharp.Threading.Tasks;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.SetActiveGameObjects
{
    public class SetActiveGameObjectsTriggerReactionFactory : ConcreteActionTriggerReactionFactory<
        SetActiveGameObjectsTriggerReactionConfig>
    {
        public override UniTask<IActionTriggerReaction> GetConcreteReactionAsync(SetActiveGameObjectsTriggerReactionConfig config)
        {
            return new UniTask<IActionTriggerReaction>(new SetActiveGameObjectsTriggerReaction(config));
        }
    }
}