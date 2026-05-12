namespace Modules.EntityModule.Runtime.Shared.Scripts.Damage.Network.Broadcasts
{
    public readonly struct InitializeDamageReceiverData
    {
        public readonly int ReceiverNetworkObjectId;
        public readonly int HealthPoints;
        public readonly int MaxHealthPoints;

        public InitializeDamageReceiverData(int receiverNetworkObjectId, int healthPoints, int maxHealthPoints)
        {
            ReceiverNetworkObjectId = receiverNetworkObjectId;
            HealthPoints = healthPoints;
            MaxHealthPoints = maxHealthPoints;
        }
    }
}