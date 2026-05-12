using Cysharp.Threading.Tasks;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Repositories;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.AliveStateTrigger
{
    public class AliveStateTriggerPredicateFactory :
        ConcreteActionTriggerPredicateFactory<AliveStateTriggerConfig>
    {
        private readonly HealthModelsRepository _healthModelsesRepository;

        public AliveStateTriggerPredicateFactory(HealthModelsRepository healthModelsesRepository)
        {
            _healthModelsesRepository = healthModelsesRepository;
        }

        public override UniTask<IActionTriggerPredicate> GetConcretePredicateAsync(
            AliveStateTriggerConfig actionTriggerConfig)
        {
            return UniTask.FromResult<IActionTriggerPredicate>(new AliveStateActionTriggerPredicate(
                actionTriggerConfig.TargetObject, actionTriggerConfig.ShouldBeAlive, _healthModelsesRepository));
        }
    }
}