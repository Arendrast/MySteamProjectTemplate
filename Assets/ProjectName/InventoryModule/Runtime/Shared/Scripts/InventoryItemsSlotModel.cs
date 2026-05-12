using System.Collections.Generic;
using System.Linq;
using ProjectName.ItemModule.Runtime.Shared.Scripts.Logic;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.InventoryModule.Runtime.Shared.Scripts
{
    public class InventoryItemsSlotModel
    {
        public IItemModel TargetItemModel => _itemModels.SafeGet(_targetItemIndex);
        public IReadOnlyList<IItemModel> ItemModels => _itemModels;
        public int TargetItemCapacity { get; private set; }
        public int ContainedTargetItemsNumber => ItemModels.Count(model => model != null);
        public int RemainingEmptyTargetItemsNumber => TargetItemCapacity - ContainedTargetItemsNumber;

        public readonly InventoryItemsSlotConfig Config;
        private readonly List<IItemModel> _itemModels = new List<IItemModel>();

        private int _targetItemIndex;

        public InventoryItemsSlotModel(InventoryItemsSlotConfig config)
        {
            Config = config;
        }

        public bool TryAddItem(IItemModel itemModel, bool shouldIgnoreContainedItems)
        {
            if (!CanAddItem(itemModel.Config, shouldIgnoreContainedItems))
                return false;

            var itemIndex = _itemModels.FindIndex(model => model == null);

            if (itemIndex == -1)
                itemIndex = 0;

            TrySetCapacity(Config.CapacityByItemType[itemModel.Config.ItemType]);

            _itemModels[itemIndex] = itemModel;

            return true;
        }
        
        public bool TryRemoveAllItems(out int removedItemsCount)
        {
            removedItemsCount = 0;

            for (var i = 0; i < _itemModels.Count; i++)
            {
                if (TryRemoveItemFromSlot(i, false))
                    removedItemsCount++;
            }

            return removedItemsCount > 0;
        }

        private bool TryRemoveItemFromSlot(int slotIndex, bool shouldShift = true)
        {
            if (_itemModels[slotIndex] == null)
                return false;
            
            Object.Destroy(_itemModels[slotIndex].LogicGameObject);

            _itemModels[slotIndex] = null;

            if (shouldShift)
                _itemModels.ShiftLeft(1);
            return true;
        }

        public bool TryRemoveTargetItem()
        {
            return TryRemoveItemFromSlot(_targetItemIndex);
        }

        public bool CanAddItem(IItemConfig itemConfig, bool shouldIgnoreContainedItems = true)
        {
            return itemConfig != null &&
                   Config.CapacityByItemType.GetValueOrDefault(itemConfig.ItemType) > 0 &&
                   (shouldIgnoreContainedItems || TargetItemModel == null ||
                    (itemConfig == TargetItemModel.Config && _itemModels.Any(model => model == null)));
        }

        public bool TrySetCapacity(int capacity)
        {
            if (capacity <= 0)
                return false;

            var different = capacity - TargetItemCapacity;
            var shouldAdd = different > 0;

            for (var i = 0; i < different; i++)
            {
                if (shouldAdd)
                {
                    _itemModels.Add(null);
                }
                else
                {
                    _itemModels.RemoveAt(_itemModels.Count - 1 - i);
                }
            }

            TargetItemCapacity = capacity;
            return true;
        }

        // public bool TrySetTargetItemIndex(int index)
        // {
        //     if (index < _itemModels.Count && index > 0)
        //     {
        //         return false;
        //     }
        //
        //     _targetItemIndex = index;
        //     return true;
        // }
    }
}