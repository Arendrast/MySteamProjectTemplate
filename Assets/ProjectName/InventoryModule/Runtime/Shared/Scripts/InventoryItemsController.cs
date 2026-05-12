using ProjectName.ItemModule.Runtime.Shared.Scripts.View;
using UnityEngine;

namespace ProjectName.InventoryModule.Runtime.Shared.Scripts
{
    public class InventoryItemsController
    {
        private readonly InventoryItemsModel _model;
        private readonly ItemsViewFactory _itemsViewFactory;
        private readonly bool _isOwner;
        private readonly Transform _viewPositionOrigin;
        private readonly Transform _viewRotationOrigin;
        private readonly ItemViewsRepository _itemViewsRepository;

        public InventoryItemsController(InventoryItemsModel model, ItemsViewFactory itemsViewFactory, bool isOwner,
            Transform viewPositionOrigin, Transform viewRotationOrigin, ItemViewsRepository itemViewsRepository)
        {
            _model = model;
            _itemsViewFactory = itemsViewFactory;
            _isOwner = isOwner;
            _viewPositionOrigin = viewPositionOrigin;
            _viewRotationOrigin = viewRotationOrigin;
            _itemViewsRepository = itemViewsRepository;

            SubscribeToModelEvents(model, itemsViewFactory);

            for (var i = 0; i < model.Config.ItemSlotsAmount; i++)
                TryCreateSlotItemViewAsync(i);
        }

        private void SubscribeToModelEvents(InventoryItemsModel model, ItemsViewFactory itemsViewFactory)
        {
            model.ChangedTargetSlot += SetItemsActiveState;
            model.AddedSlotItem += TryCreateSlotItemViewAsync;
            model.RemovedSlotItem += (slotIndex, onlyOne) =>
            {
                if (model.SlotsContainedTargetItemsNumber[slotIndex] > 0)
                    TryCreateSlotItemViewAsync(slotIndex);
            };

            return;

            void SetItemsActiveState(int targetSlotIndex)
            {
                for (var i = 0; i < model.Config.ItemSlotsAmount; i++)
                    SetItemActiveState(i);
            }
        }

        private async void TryCreateSlotItemViewAsync(int slotIndex)
        {
            var itemModel = _model.TryGetItemModel(slotIndex);

            if (itemModel == null)
                return;
            
            itemModel.LogicGameObject.transform.localPosition = itemModel.Config.StartLocalPosition;
            itemModel.LogicGameObject.transform.localRotation = Quaternion.Euler(itemModel.Config.StartLocalRotation);

            var instance =
                await _itemsViewFactory.GetItemViewInstanceAsync(itemModel, _viewPositionOrigin, _viewRotationOrigin,
                    _isOwner, _viewPositionOrigin);
            
            SetItemActiveState(slotIndex);
        }

        private void SetItemActiveState(int slotIndex)
        {
            var instance = _model.TryGetItemModel(slotIndex)?.LogicGameObject;

            if (!instance)
                return;

            var gameObject = _model.TryGetItemModel(slotIndex).LogicGameObject;

            var isActive = slotIndex == _model.TargetSlotIndex;
            
            gameObject.SetActive(isActive);

            if (_itemViewsRepository.TryGetValue(gameObject.GetInstanceID(), out var components))
            {
                components.gameObject.SetActive(isActive);
            }
        }
    }
}