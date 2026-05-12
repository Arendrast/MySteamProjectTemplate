using FishNet.Broadcast;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Damage.Network.Broadcasts
{
    public readonly struct InitializeDamageReceiversBroadcast : IBroadcast
    {
        public readonly InitializeDamageReceiverData[] ReceiversData;

        public InitializeDamageReceiversBroadcast(InitializeDamageReceiverData[] receiversData)
        {
            ReceiversData = receiversData;
        }
    }
}