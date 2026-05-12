using FishNet.Broadcast;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Heal.Network.Broadcasts
{
    public readonly struct InitializeHealReceiversBroadcast : IBroadcast
    {
        public readonly InitializeHealReceiverData[] ReceiversData;

        public InitializeHealReceiversBroadcast(InitializeHealReceiverData[] receiversData)
        {
            ReceiversData = receiversData;
        }
    }
}