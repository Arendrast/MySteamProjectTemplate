using FishNet.Broadcast;

namespace ProjectName.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public struct AddSlotItemBroadcastForServer : IBroadcast
    {
        public readonly int ItemId;
        public readonly int SlotIndex;

        public AddSlotItemBroadcastForServer(int itemId, int slotIndex)
        {
            ItemId = itemId;
            SlotIndex = slotIndex;
        }
    }
}