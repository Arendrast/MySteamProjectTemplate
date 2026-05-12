using FishNet.Broadcast;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Heal.Network.Broadcasts
{
    public struct DealHealBroadcast : IBroadcast
    {
        public readonly int ReceiverNetworkObjectId;
        public readonly DoHealData DoHealData;

        public DealHealBroadcast(int receiverNetworkObjectId, DoHealData doHealData)
        {
            ReceiverNetworkObjectId = receiverNetworkObjectId;
            DoHealData = doHealData;
        }
    }
}