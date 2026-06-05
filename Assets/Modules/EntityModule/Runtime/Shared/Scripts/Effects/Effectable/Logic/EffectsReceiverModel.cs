using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic
{
    public class EffectsReceiverModel : IEffectable
    {
        public int Id { get; }
        public event Action<EffectType, int, EffectOrigin> AppliedEffect;
        public event Action<EffectType, int> CancelledEffect;
        
        public IReadOnlyCollection<EffectType> ActiveEffects => _activeEffects;
        public IReadOnlyDictionary<EffectType, IEffectReactor> EffectReactors { get; }

        private readonly HashSet<EffectType> _activeEffects = new HashSet<EffectType>();

        private readonly Dictionary<EffectType, CancellationTokenSource> _activeCancelDelayTokensSources = new();

        public EffectsReceiverModel(IReadOnlyDictionary<EffectType, IEffectReactor> effectReactors, int id)
        {
            EffectReactors = effectReactors;
            Id = id;
        }

        public bool TryApplyEffect(EffectType effectType, int effectApplierId, EffectOrigin effectOrigin)
        {
            if (_activeEffects.Contains(effectType))
            {
                return false;
            }
            
            if (_activeCancelDelayTokensSources.Remove(effectType, out var tokenSource))
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
            
            EffectReactors.GetValueOrDefault(effectType)?.OnApply(_activeEffects, effectApplierId);
            _activeEffects.Add(effectType);
            AppliedEffect?.Invoke(effectType, effectApplierId, effectOrigin);

            return true;
        }

        public bool TryCancelEffect(EffectType effectType, int effectCancellerId, float delay = 0)
        {
            if (!_activeEffects.Contains(effectType))
            {
                return false;
            }
            
            CancelEffectAfterDelayAsync(effectType, delay, effectCancellerId);
            return true;
        }

        private async void CancelEffectAfterDelayAsync(EffectType effectType, float delay, int effectCancellerId)
        {
            if (_activeCancelDelayTokensSources.Remove(effectType, out var pastTokenSource))
            {
                pastTokenSource.Cancel();
                pastTokenSource.Dispose();
            }
            
            if (delay > 0)
            {
                var tokenSource = new CancellationTokenSource();
                
                _activeCancelDelayTokensSources.Add(effectType, tokenSource);

                if (await AsyncTools.AwaitTaskAndGetDoesThrowOperationCancelledException(
                        UniTask.WaitForSeconds(delay, cancellationToken: tokenSource.Token, cancelImmediately: true)))
                {
                    _activeCancelDelayTokensSources.Remove(effectType);
                    return;
                }

                _activeCancelDelayTokensSources.Remove(effectType);
            }
            
            _activeEffects.Remove(effectType);
            EffectReactors.GetValueOrDefault(effectType)?.OnCancel(_activeEffects);
            
            CancelledEffect?.Invoke(effectType, effectCancellerId);
        }
    }
}