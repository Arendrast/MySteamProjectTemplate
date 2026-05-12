using System;
using System.Linq;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Destroyable.Logic
{
    public class DestructionObserver
    {
        public DestructionStatesConfig.DestructionStateConfig CurrentConfig { get; private set; }
        
        public event Action<DestructionStatesConfig.DestructionStateConfig> ChangedDestructionState;

        public DestructionObserver(HealthModel healthModel, DestructionStatesConfig destructionStatesConfig)
        {
            healthModel.ChangedHealthPoints += TryInvokeChangedDestructionState;
            TryInvokeChangedDestructionState(healthModel.HealthPoints);

            return;

            void TryInvokeChangedDestructionState(int currentHealth)
            {
                var greaterConfigs =
                    destructionStatesConfig.DestructionStateConfigs.Where(config =>
                        config.BorderHealthNumber >= currentHealth).ToList();
                
                if (!greaterConfigs.Any())
                    return;

                CurrentConfig = greaterConfigs.OrderBy(config => config.BorderHealthNumber).First();
                
                ChangedDestructionState?.Invoke(CurrentConfig);
            }
        }
    }
}