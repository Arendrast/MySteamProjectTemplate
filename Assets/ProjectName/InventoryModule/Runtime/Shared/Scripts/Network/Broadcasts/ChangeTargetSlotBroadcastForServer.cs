using FishNet.Broadcast;

namespace ProjectName.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public struct ChangeTargetSlotBroadcastForServer : IBroadcast
    {
        public readonly int SlotIndex;

        public ChangeTargetSlotBroadcastForServer(int slotIndex) => SlotIndex = slotIndex;
    }
}