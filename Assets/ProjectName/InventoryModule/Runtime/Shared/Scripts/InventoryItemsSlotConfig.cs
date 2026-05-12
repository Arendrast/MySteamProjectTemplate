using System;
using System.Collections.Generic;
using ProjectName.ItemModule.Runtime.Shared.Scripts.Logic;
using Sirenix.Serialization;

namespace ProjectName.InventoryModule.Runtime.Shared.Scripts
{
    [Serializable]
    public class InventoryItemsSlotConfig 
    {
        [field: OdinSerialize] public IItemConfig DefaultItemConfig { get; private set; }
        [field: OdinSerialize] public IReadOnlyDictionary<ItemType, int> CapacityByItemType { get; private set; }
    }
}