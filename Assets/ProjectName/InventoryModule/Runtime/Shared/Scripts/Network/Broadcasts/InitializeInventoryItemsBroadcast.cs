using FishNet.Broadcast;

namespace ProjectName.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public readonly struct InitializeInventoryItemsData : IBroadcast
    {
        public readonly int TargetSlotIndex;
        public readonly ItemSlotData[] ItemSlotsData;

        public InitializeInventoryItemsData(int targetSlotIndex, ItemSlotData[] itemSlotsData)
        {
            ItemSlotsData = itemSlotsData;
            TargetSlotIndex = targetSlotIndex;
        }
    }
}