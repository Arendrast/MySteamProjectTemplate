using System;
using System.Collections.Generic;
using Modules.ItemModule.Runtime.Shared.Scripts.Logic;
using Sirenix.Serialization;

namespace Modules.InventoryModule.Runtime.Shared.Scripts
{
    [Serializable]
    public class InventoryItemsSlotConfig 
    {
        [field: OdinSerialize] public IItemConfig DefaultItemConfig { get; private set; }
        [field: OdinSerialize] public IReadOnlyDictionary<ItemType, int> CapacityByItemType { get; private set; }
    }
}