using Cysharp.Threading.Tasks;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters
{
    public class UpdateCountTriggerReactionFactory : ConcreteActionTriggerReactionFactory<
        UpdateCounterTriggerReactionConfig>
    {
        private readonly NetworkCountersSynchronizerBehaviourRepository _behaviourRepository;

        public UpdateCountTriggerReactionFactory(NetworkCountersSynchronizerBehaviourRepository behaviourRepository)
        {
            _behaviourRepository = behaviourRepository;
        }

        public override UniTask<IActionTriggerReaction> GetConcreteReactionAsync(UpdateCounterTriggerReactionConfig config)
        {
            return new UniTask<IActionTriggerReaction>(new UpdateCountTriggerReaction(_behaviourRepository, config));
        }
    }
}