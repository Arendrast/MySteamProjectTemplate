using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Modules.ItemModule.Runtime.Shared.Scripts.Logic;
using Timer = Modules.SharedModule.Runtime.Shared.Scripts.QoL.Timer;

namespace Modules.InventoryModule.Runtime.Shared.Scripts
{
    public class InventoryItemsModel
    {
        public IReadOnlyList<IItemModel> SlotsTargetItemModels => _slots.Select(slot => slot.TargetItemModel).ToList();

        public IReadOnlyList<int> SlotsContainedTargetItemsNumber =>
            _slots.Select(slot => slot.ContainedTargetItemsNumber).ToList();

        public IReadOnlyList<int> SlotsRemainingEmptyTargetItemsNumber =>
            _slots.Select(slot => slot.RemainingEmptyTargetItemsNumber).ToList();

        public IItemModel TargetItemModel => _slots[TargetSlotIndex].TargetItemModel;

        public bool DoesChangeTargetItem =>
            _setTargetSlotTimer.IsCounting() || _setTargetSlotTargetItemIndexTimer.IsCounting();
        
        public int TargetSlotIndex { get; private set; }
        public int NextTargetSlotIndex { get; private set; }
        public InventoryItemsConfig Config { get; }

        public event Action<int, int, bool> StartedChangeTargetSlot;
        public event Action<int> StartedChangeSlotTargetItem;
        public event Action<int> ChangedTargetSlot;
        public event Action<int, bool> RemovedSlotItem;
        public event Action<int> AddedSlotItem;

        private bool _canUseTargetItem = true;

        private readonly List<InventoryItemsSlotModel> _slots;

        private readonly Timer _setTargetSlotTimer, _setTargetSlotTargetItemIndexTimer, _startSetProcessTargetSlotTimer;


        public InventoryItemsModel(CancellationToken cancellationToken, InventoryItemsConfig config)
        {
            Config = config;
            _setTargetSlotTimer = new Timer(cancellationToken);
            _setTargetSlotTargetItemIndexTimer = new Timer(cancellationToken);
            _startSetProcessTargetSlotTimer = new Timer(cancellationToken);
            _slots = Config.ItemSlotsConfigs.Select(config => new InventoryItemsSlotModel(config)).ToList();

            TargetSlotIndex = config.StartTargetSlotIndex;
            NextTargetSlotIndex = TargetSlotIndex;
            
            SubscribeSetTargetSlotTimer();
        }

        public bool CanUseTargetItem(InterruptReason interruptReason)
        {
            return _canUseTargetItem && TargetItemModel != null && TargetItemModel.CanInterruptLogic(interruptReason) &&
                   !DoesChangeTargetItem;
        }

        public IItemModel TryGetItemModel(int slotIndex)
        {
            return IsValidSlotIndex(slotIndex) ? _slots[slotIndex].TargetItemModel : null;
        }

        public bool TryAddItemToSlot(IItemModel itemModel, int slotIndex)
        {
            if (!IsValidSlotIndex(slotIndex) || !_slots[slotIndex].TryAddItem(itemModel,
                    false))
                return false;
            
            AddedSlotItem?.Invoke(slotIndex);
            return true;
        }

        public void SetCanUseTargetItem(bool value)
        {
            _canUseTargetItem = value;
        }

        // public bool TrySetSlotTargetItemIndex(int slotIndex, int index)
        // {
        //     return IsValidSlotIndex(slotIndex) && _slots[slotIndex].TrySetTargetItemIndex(index);
        // }

        public bool CanAddItemToAnySlot(out int slotIndex, IItemConfig itemConfig,
            bool shouldIgnoreCurrentSlotItem = true)
        {
            slotIndex = 0;

            for (var i = 0; i < Config.ItemSlotsAmount; i++)
            {
                slotIndex = i;

                if (CanAddItemToSlot(i, itemConfig, shouldIgnoreCurrentSlotItem))
                    return true;
            }

            return false;
        }

        private bool CanAddItemToSlot(int slotIndex, IItemConfig itemConfig, bool shouldIgnoreContainedItems = true)
        {
            return IsValidSlotIndex(slotIndex) &&
                   _slots[slotIndex].CanAddItem(itemConfig, shouldIgnoreContainedItems);
        }

        public bool CanRemoveItemFromSlot(int slotIndex, bool shouldCheckCanUseTargetItem)
        {
            return IsValidSlotIndex(slotIndex) &&
                   (slotIndex != TargetSlotIndex ||
                    TargetItemModel == null ||
                    !shouldCheckCanUseTargetItem || CanUseTargetItem(InterruptReason.Remove));
        }

        public bool TryRemoveItemsFromSlot(int slotIndex, bool shouldCheckCanUseTargetItem, bool onlyOne)
        {
            return TryRemoveItemsFromSlot(slotIndex, shouldCheckCanUseTargetItem, onlyOne, out var removedItemsCount);
        }

        public bool TryRemoveItemsFromSlot(int slotIndex, bool shouldCheckCanUseTargetItem, bool onlyOne,
            out int removedItemsCount)
        {
            removedItemsCount = 0;

            if (!CanRemoveItemFromSlot(slotIndex, shouldCheckCanUseTargetItem) || (onlyOne
                    ? !_slots[slotIndex].TryRemoveTargetItem()
                    : !_slots[slotIndex].TryRemoveAllItems(out removedItemsCount)))
                return false;

            if (onlyOne)
                removedItemsCount = 1;

            if (_slots[slotIndex].ContainedTargetItemsNumber > 0)
                SetTargetSlotTargetItem();

            RemovedSlotItem?.Invoke(slotIndex, onlyOne);
            return true;
        }

        public bool TryRemoveItemsFromTargetSlot(bool shouldCheckCanUseTargetItem, bool onlyOne)
        {
            return TryRemoveItemsFromSlot(TargetSlotIndex, shouldCheckCanUseTargetItem, onlyOne,
                out var removedItemsCount);
        }

        public void StopSetTargetSlotAndTargetSlotTargetItemIndexTimers()
        {
            NextTargetSlotIndex = TargetSlotIndex;
            _setTargetSlotTargetItemIndexTimer.TryStopCountingTime();
            _setTargetSlotTimer.TryStopCountingTime();
        }

        public void StartSetTargetSlot(int slotIndex, bool shouldSkipTimer = false)
        {
            StopSetTargetSlotAndTargetSlotTargetItemIndexTimers();
            NextTargetSlotIndex = slotIndex;
            
            StartedChangeTargetSlot?.Invoke(TargetSlotIndex, NextTargetSlotIndex, shouldSkipTimer);

            if (shouldSkipTimer)
            {
                SetTargetSlot();
            }
            else
            {
                var totalTime = TryGetRemoveTimeForSlot(TargetSlotIndex) + TryGetAddTimeForSlot(NextTargetSlotIndex);
                _setTargetSlotTimer.TryStartCountingTime(totalTime);
            }

            TargetItemModel?.InterruptUsing();
        }

        public int GetFirstNotEmptySlotIndex()
        {
            for (var i = 0; i < SlotsTargetItemModels.Count; i++)
            {
                if (SlotsTargetItemModels[i] != null)
                    return i;
            }

            return SlotsTargetItemModels.Count - 1;
        }

        public bool TryStartToSetTargetSlot(int slotIndex, bool shouldSkipTimer = false)
        {
            if ((TargetItemModel != null &&
                 (!_canUseTargetItem || !TargetItemModel.CanInterruptLogic(InterruptReason.SetItem))) ||
                !IsValidSlotIndex(slotIndex) || SlotsContainedTargetItemsNumber[slotIndex] == 0 ||
                slotIndex == TargetSlotIndex ||
                slotIndex == NextTargetSlotIndex || (!shouldSkipTimer && _startSetProcessTargetSlotTimer.IsCounting()))
                return false;
            
            if (!shouldSkipTimer && _setTargetSlotTimer.IsCounting() && 
                _setTargetSlotTimer.PastTime < Config.TimeBeforeStartSetTargetSlotWhenSetTargetSlot)
            {
                _startSetProcessTargetSlotTimer.ClearEnded();
                _startSetProcessTargetSlotTimer.Ended += LocalStartSetProcessTargetSlot;
                _setTargetSlotTargetItemIndexTimer.TryStopCountingTime();
                _setTargetSlotTimer.TryStopCountingTime();
                
                _startSetProcessTargetSlotTimer.TryStartCountingTime(Config.TimeBeforeStartSetTargetSlotWhenSetTargetSlot -
                                                                     _setTargetSlotTimer.PastTime);
                
                return true;
            }

            LocalStartSetProcessTargetSlot();
            return true;

            void LocalStartSetProcessTargetSlot()
            {
                StartSetTargetSlot(slotIndex, shouldSkipTimer);
            }
        }

        private void SetTargetSlotTargetItem()
        {
            var totalTime = TryGetAddTimeForSlot(NextTargetSlotIndex);
            _setTargetSlotTargetItemIndexTimer.TryStartCountingTime(totalTime);
            StartedChangeSlotTargetItem?.Invoke(TargetSlotIndex);
        }

        private bool IsValidSlotIndex(int slotIndex) => slotIndex >= 0 && slotIndex < _slots.Count;

        private void SubscribeSetTargetSlotTimer()
        {
            _setTargetSlotTimer.Ended += SetTargetSlot;
        }

        private void SetTargetSlot()
        {
            TargetSlotIndex = NextTargetSlotIndex;
            ChangedTargetSlot?.Invoke(TargetSlotIndex);
        }

        private float TryGetRemoveTimeForSlot(int slotIndex)
        {
            var slot = TryGetItemModel(slotIndex);
            return slot != null ? slot.Config.ItemOnRemoveFromSlotTime : 0;
        }

        private float TryGetAddTimeForSlot(int slotIndex)
        {
            var slot = TryGetItemModel(slotIndex);
            return slot != null ? slot.Config.ItemOnAddToSlotTime : 0;
        }
    }
}