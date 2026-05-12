using System;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Heal
{
    public class HealReceiverModel
    {
        public int MaxHealthPoints => _healthModel.MaxHealthPoints;
        public int HealthPoints => _healthModel.HealthPoints;

        public event Action<DoHealData> BeforeHeal;
        public event Action<DoHealData, int> Healed;

        private Func<bool> _canHealFunc;

        private readonly HealthModel _healthModel;

        public HealReceiverModel(HealthModel healthModel)
        {
            _healthModel = healthModel;
        }

        public void SetCanHealFunc(Func<bool> canHealFunc)
        {
            _canHealFunc = canHealFunc;
        }

        public void TryHeal(DoHealData doHealData)
        {
            if (_canHealFunc != null && !_canHealFunc.Invoke())
                return;

            BeforeHeal?.Invoke(doHealData);

            _healthModel.TrySetHealthPoints(_healthModel.HealthPoints + doHealData.Heal, doHealData.HealDealerId, out var healthPointsDifference,
                doHealData.CheckDeath, doHealData.OverridedMaxHealPoints);
            
            Healed?.Invoke(doHealData, healthPointsDifference);
        }
    }
}