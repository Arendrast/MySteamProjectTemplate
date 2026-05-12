using System;
using System.Collections.Generic;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.SharedModule.Runtime.Shared.Scripts.Index;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Damage
{
    public class DamageReceiverModel : IIndexable
    {
        public int Id { get; }
        public bool IsDead => _healthModel.IsDied;

        public event Action<DoDamageData, int> BeforeReceiveDamage;
        public event Action<DoDamageData, int> ReceivedDamage;

        private readonly HashSet<IDamageHandler> _damageHandlers = new HashSet<IDamageHandler>();
        private readonly HealthModel _healthModel;

        public DamageReceiverModel(int id, HealthModel healthModel)
        {
            Id = id;
            _healthModel = healthModel;
        }

        public void TryAddDamageHandler(IDamageHandler damageHandler)
        {
            _damageHandlers.Add(damageHandler);
        }

        public void TryRemoveDamageHandler(IDamageHandler damageHandler)
        {
            _damageHandlers.Remove(damageHandler);
        }

        public void TryTakeDamage(DoDamageData doDamageData, out int tookDamage)
        {
            tookDamage = 0;

            if (doDamageData.Damage <= 0)
                return;

            var resultDamage = doDamageData.Damage;

            foreach (var damageHandler in _damageHandlers)
            {
                resultDamage -= damageHandler.GetDifferentDamage(doDamageData.Damage);
            }

            var predictedDamage = Mathf.Clamp(resultDamage, 0, _healthModel.HealthPoints);

            //Debug.Log($"[TakeDamage] {this} took {tookDamage} dmg (raw={doDamageData.Damage}), HP: {_healthModel.HealthPoints} -> {_healthModel.HealthPoints - tookDamage}\n{new System.Diagnostics.StackTrace(1, false)}");

            BeforeReceiveDamage?.Invoke(doDamageData, predictedDamage);

            _healthModel.TrySetHealthPoints(_healthModel.HealthPoints - predictedDamage,
                setterId: doDamageData.DamageDealerId, out tookDamage);

            ReceivedDamage?.Invoke(doDamageData, tookDamage);
        }

        public override string ToString()
        {
            return $"{nameof(DamageReceiverModel)}_{_healthModel.Name}_{Id}";
        }
    }
}