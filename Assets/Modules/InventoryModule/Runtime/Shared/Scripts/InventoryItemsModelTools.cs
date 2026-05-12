using System.Collections.Generic;
using Modules.ItemModule.Runtime.Shared.Scripts.Logic;
using Modules.SharedModule.Runtime.Shared.Scripts.Index;

namespace Modules.InventoryModule.Runtime.Shared.Scripts
{
    public static class InventoryItemsModelTools
    {
        public static void SetSlotByItemType(this InventoryItemsModel itemsModel, ItemType itemType,
            bool shouldSkipTimer, out int slotIndex, bool force)
        {
            slotIndex = GetSlotIndexByItemType(itemsModel, itemType);

            if (slotIndex == IndexableTools.MissingOrInvalidId)
                return;

            if (force)
                itemsModel.StartSetTargetSlot(slotIndex, shouldSkipTimer);
            else
                itemsModel.TryStartToSetTargetSlot(slotIndex, shouldSkipTimer);
        }

        public static int GetSlotIndexByItemType(this InventoryItemsModel itemsModel, ItemType itemType)
        {
            var slotIndex = IndexableTools.MissingOrInvalidId;

            for (var i = 0; i < itemsModel.Config.ItemSlotsAmount; i++)
            {
                if (itemsModel.Config.ItemSlotsConfigs[i].CapacityByItemType.GetValueOrDefault(itemType) <= 0) continue;
                slotIndex = i;
                break;
            }

            return slotIndex;
        }
    }
}