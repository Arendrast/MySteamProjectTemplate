using FishNet.Broadcast;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Damage.Network.Broadcasts
{
    public struct DealDamageBroadcast : IBroadcast
    {
        public readonly int ReceiverNetworkObjectId;
        public readonly DoDamageData DoDamageData;

        public DealDamageBroadcast(int receiverNetworkObjectId, DoDamageData doDamageData)
        {
            ReceiverNetworkObjectId = receiverNetworkObjectId;
            DoDamageData = doDamageData;
        }
    }
}