using System.Collections.Generic;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic
{
    public class EffectsReceiverController
    {
        private readonly List<EffectType> _activeEffects = new List<EffectType>();
        
        public EffectsReceiverController(EffectsReceiverModel model, GameObject gameObject, UpdateObserversService updateObserversService)
        {
            updateObserversService.TryAddOrGetUpdateObserver(gameObject, UpdateType.Update, out var updateObserver);
            updateObserver.Updated += UpdateEffectsReactors;
            
            return;

            void UpdateEffectsReactors(float time)
            {
                _activeEffects.Clear();
                _activeEffects.AddRange(model.ActiveEffects);
                
                foreach (var effect in _activeEffects)
                {
                    model.EffectReactors.GetValueOrDefault(effect)?.OnUpdate(time);
                }
            }
        }
    }
}