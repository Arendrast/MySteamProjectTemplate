using FishNet.Broadcast;

namespace ProjectName.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public struct RemoveSlotItemBroadcastForClient : IBroadcast
    {
        public readonly int FromNetworkConnectionId;
        public readonly int SlotIndex;
        public readonly bool OnlyOne;
        
        public RemoveSlotItemBroadcastForClient(int fromNetworkConnectionId, int slotIndex, bool onlyOne)
        {
            FromNetworkConnectionId = fromNetworkConnectionId;
            SlotIndex = slotIndex;
            OnlyOne = onlyOne;
        }
    }
}