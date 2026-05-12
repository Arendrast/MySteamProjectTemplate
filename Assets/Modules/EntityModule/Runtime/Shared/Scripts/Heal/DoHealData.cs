namespace Modules.EntityModule.Runtime.Shared.Scripts.Heal
{
    public struct DoHealData
    {
        public readonly int Heal;
        public readonly HealOrigin HealOrigin;
        public readonly int? OverridedMaxHealPoints;
        public readonly bool CheckDeath;
        public readonly int HealDealerId;

        public DoHealData(int heal, HealOrigin healOrigin, int? overridedMaxHealPoints = null, bool checkDeath = true,
            int healDealerId = -1)
        {
            Heal = heal;
            CheckDeath = checkDeath;
            OverridedMaxHealPoints = overridedMaxHealPoints;
            HealOrigin = healOrigin;
            HealDealerId = healDealerId;
        }

        public DoHealData WithHealDealerId(int healDealerId)
        {
            return new DoHealData(Heal, HealOrigin, OverridedMaxHealPoints, CheckDeath, healDealerId);
        }
    }
}