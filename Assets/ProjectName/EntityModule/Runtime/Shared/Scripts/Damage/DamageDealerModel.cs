using System;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Damage
{
    public class DamageDealerModel
    {
        public event Action<DamageReceiverModel, DoDamageData> BeforeDealDamage;

        private readonly int _id;

        public DamageDealerModel(int id)
        {
            _id = id;
        }

        public void DoDamage(DamageReceiverModel damageReceiverModel, DoDamageData doDamageData,
            int? overridedDamageDealerId = null)
        {
            BeforeDealDamage?.Invoke(damageReceiverModel, doDamageData);
            damageReceiverModel.TryTakeDamage(doDamageData.WithDamageDealerId(overridedDamageDealerId ?? _id), out var tookDamage);
        }
    }
}