using FishNet.Broadcast;

namespace Modules.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public struct RemoveSlotItemBroadcastForServer : IBroadcast
    {
        public readonly int SlotIndex;
        public readonly bool OnlyOne;


        public RemoveSlotItemBroadcastForServer(int slotIndex, bool onlyOne)
        {
            SlotIndex = slotIndex;
            OnlyOne = onlyOne;
        }
    }
}