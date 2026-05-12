namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Heal
{
    public readonly struct InitializeHealReceiverData
    {
        public readonly int ReceiverNetworkObjectId;
        public readonly int HealthPoints;
        public readonly int MaxHealthPoints;

        public InitializeHealReceiverData(int receiverNetworkObjectId, int healthPoints, int maxHealthPoints)
        {
            ReceiverNetworkObjectId = receiverNetworkObjectId;
            HealthPoints = healthPoints;
            MaxHealthPoints = maxHealthPoints;
        }
    }
}