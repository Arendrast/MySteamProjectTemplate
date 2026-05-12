using Cysharp.Threading.Tasks;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters
{
    public class UpdateCountTriggerReaction : IActionTriggerReaction
    {
        private readonly NetworkCountersSynchronizerBehaviourRepository _behaviourRepository;
        private readonly UpdateCounterTriggerReactionConfig _config;

        public UpdateCountTriggerReaction(NetworkCountersSynchronizerBehaviourRepository behaviourRepository, UpdateCounterTriggerReactionConfig config)
        {
            _behaviourRepository = behaviourRepository;
            _config = config;
        }

        public async void Invoke()
        {
            await UniTask.WaitWhile(() => _behaviourRepository.Behaviour == null);
            
            if (!_config.CanUpdateIfValueIsLessOrEqual && 
                _behaviourRepository.Behaviour.Counters.TryGetValue(_config.CounterType, out var value) &&
                value >= _config.Value)
                return;
            
            _behaviourRepository.Behaviour.TryUpdateValue(_config.CounterType, _config.Value);
        }
    }
}