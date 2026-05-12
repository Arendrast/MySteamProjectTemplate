namespace ProjectName.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public readonly struct ItemSlotData
    {
        public readonly int ItemId;
        public readonly int Count;

        public ItemSlotData(int itemId, int count)
        {
            ItemId = itemId;
            Count = count;
        }
    }
}