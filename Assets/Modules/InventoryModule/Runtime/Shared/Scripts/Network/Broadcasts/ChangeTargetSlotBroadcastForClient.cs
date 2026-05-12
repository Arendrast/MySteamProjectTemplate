using FishNet.Broadcast;

namespace Modules.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public struct ChangeTargetSlotBroadcastForClient : IBroadcast
    {
        public readonly int FromNetworkConnectionId;
        public readonly int SlotIndex;
        
        public ChangeTargetSlotBroadcastForClient(int fromNetworkConnectionId, int slotIndex)
        {
            FromNetworkConnectionId = fromNetworkConnectionId;
            SlotIndex = slotIndex;
        }
    }
}