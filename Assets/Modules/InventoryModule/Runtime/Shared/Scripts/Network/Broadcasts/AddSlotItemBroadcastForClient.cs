using FishNet.Broadcast;

namespace Modules.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public struct AddSlotItemBroadcastForClient : IBroadcast
    {
        public readonly int FromNetworkConnectionId;
        public readonly int ItemId;
        public readonly int SlotIndex;

        public AddSlotItemBroadcastForClient(int fromNetworkConnectionId, int itemId, int slotIndex)
        {
            ItemId = itemId;
            SlotIndex = slotIndex;
            FromNetworkConnectionId = fromNetworkConnectionId;
        }
    }
}