using System;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors.IntermittentDamage
{
    public class IntermittentDamageReactor
    {
        public event Action DealtDamage;
        
        private int _effectApplierId;
        private float _remainingTime;
        
        private readonly DamageDealerModel _damageDealerModel;
        private readonly DamageReceiverModel _damageReceiverModel;
        private readonly IntermittentDamageReactorConfig _config;
        private readonly DamageOrigin _damageOrigin;

        public IntermittentDamageReactor(DamageDealerModel damageDealerModel,
            DamageReceiverModel damageReceiverModel, IntermittentDamageReactorConfig config, DamageOrigin damageOrigin)
        {
            _damageDealerModel = damageDealerModel;
            _damageReceiverModel = damageReceiverModel;
            _config = config;
            _damageOrigin = damageOrigin;
        }

        public void OnApply(int effectApplierId)
        {
            _effectApplierId = effectApplierId;
            UpdateRemainingTime();
        }

        public void OnUpdate()
        {
            _remainingTime -= Time.deltaTime;

            if (_remainingTime > 0) return;

            _damageDealerModel.DoDamage(_damageReceiverModel, new DoDamageData(_config.DamagePerTick, _damageOrigin),
                _effectApplierId);
            UpdateRemainingTime();

            DealtDamage?.Invoke();
        }

        public void OnCancel()
        {
        }

        private void UpdateRemainingTime()
        {
            _remainingTime = 1f / _config.TicksPerSecond;
        }
    }
}