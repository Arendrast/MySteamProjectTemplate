using System.Collections.Generic;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic
{
    public class EffectsReceiverController
    {
        private List<EffectType> _activeEffects = new List<EffectType>();
        
        public EffectsReceiverController(EffectsReceiverModel model, MonoBehaviourObserver monoBehaviourObserver)
        {
            monoBehaviourObserver.Updated += UpdateEffectsReactors;
            
            return;

            void UpdateEffectsReactors()
            {
                _activeEffects.Clear();
                _activeEffects.AddRange(model.ActiveEffects);
                
                foreach (var effect in _activeEffects)
                {
                    model.EffectReactors.GetValueOrDefault(effect)?.OnUpdate();
                }
            }
        }
    }
}