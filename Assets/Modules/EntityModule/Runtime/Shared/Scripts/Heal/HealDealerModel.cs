using System;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Heal
{
    public class HealDealerModel
    {
        public readonly int Id;

        public event Action<HealReceiverModel, DoHealData> BeforeDealHeal;

        public HealDealerModel(int id)
        {
            Id = id;
        }

        public void DoHeal(HealReceiverModel healReceiverModel, DoHealData doHealData)
        {
            BeforeDealHeal?.Invoke(healReceiverModel,doHealData);
            healReceiverModel.TryHeal(doHealData.WithHealDealerId(Id));
        }
    }
}