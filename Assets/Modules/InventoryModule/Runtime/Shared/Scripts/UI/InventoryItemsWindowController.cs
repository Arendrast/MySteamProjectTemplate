using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Modules.ItemModule.Runtime.Shared.Scripts.Logic;
using Modules.ItemModule.Runtime.Shared.Scripts.View;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using MoreLinq;

namespace Modules.InventoryModule.Runtime.Shared.Scripts.UI
{
    public class InventoryItemsWindowController
    {
        private readonly List<Tween> _tweens = new List<Tween>();

        public InventoryItemsWindowController(InventoryItemsWindowSerializableComponents window,
            InventoryItemsModel inventoryItemsModel,
            ItemsViewConfig itemsViewConfig)
        {
            inventoryItemsModel.ChangedTargetSlot += _ => UpdateAllSlotsView();
            inventoryItemsModel.ChangedTargetSlot += _ => StartDelayedDoFade();
            inventoryItemsModel.StartedChangeTargetSlot += (_, _, _) => ReturnStatAlphaAndKillTweens();
            inventoryItemsModel.RemovedSlotItem += (slotIndex, _) => TryMakeSlotEmpty(slotIndex);
            inventoryItemsModel.RemovedSlotItem += (slotIndex, _) => UpdateSlotView(slotIndex);
            inventoryItemsModel.AddedSlotItem += UpdateSlotView;

            UpdateAllSlotsView();
            StartDelayedDoFade();

            return;

            void ReturnStatAlphaAndKillTweens()
            {
                window.BackgroundImage.color = window.BackgroundImage.color.WithA(1);
                window.InventoryItemsBlockSerializableComponents.ForEach(components => components.ItemImage.color =
                    components.ItemImage.color.WithA(1));

                _tweens.ForEach(tween => tween?.Kill());
                _tweens.Clear();
            }

            void StartDelayedDoFade()
            {
                _tweens.Add(window.BackgroundImage.transform.DOMove(window.BackgroundImage.transform.position,
                    window.TimeBeforeDoFade).OnComplete(DoFade));

                return;

                void DoFade()
                {
                    _tweens.Add(window.BackgroundImage.DOFade(window.FadedBackgroundAlphaValue,
                        window.FadeTime));

                    _tweens.AddRange(window.InventoryItemsBlockSerializableComponents.Select((components, index) =>
                        index == inventoryItemsModel.TargetSlotIndex
                            ? null
                            : components.ItemImage.DOFade(
                                window.FadedItemsImagesAlphaValue, window.FadeTime)));
                }
            }

            void UpdateAllSlotsView()
            {
                for (var i = 0; i < inventoryItemsModel.Config.ItemSlotsAmount; i++)
                {
                    UpdateSlotView(i);
                }
            }

            void UpdateSlotView(int slotIndex)
            {
                var config = inventoryItemsModel.SlotsTargetItemModels[slotIndex]?.Config;

                if (config == null)
                {
                    TryMakeSlotEmpty(slotIndex);
                    return;
                }
                
                var itemViewConfig = itemsViewConfig.ItemConfigs.FirstOrDefault(localConfig =>
                    localConfig?.Id == config?.Id);

                if (itemViewConfig == null)
                {
                    TryMakeSlotEmpty(slotIndex);
                    return;
                }

                window.InventoryItemsBlockSerializableComponents[slotIndex].ItemImage.gameObject.SetActive(true);
                window.InventoryItemsBlockSerializableComponents[slotIndex].ItemsCountText.gameObject.SetActive(true);

                var containedItemsNumber = inventoryItemsModel.SlotsContainedTargetItemsNumber[slotIndex];

                window.InventoryItemsBlockSerializableComponents[slotIndex].ItemsCountText.text =
                    config.ItemType.IsGrenadeOrKitOrCan() ? containedItemsNumber + "x" : "";

                window.InventoryItemsBlockSerializableComponents[slotIndex].ItemImage.sprite =
                    inventoryItemsModel.TargetSlotIndex == slotIndex
                        ? itemViewConfig.SharedItemViewConfig.SelectedItemSprite
                        : itemViewConfig.SharedItemViewConfig.DeselectedItemSprite;
            }

            void TryMakeSlotEmpty(int targetSlotIndex)
            {
                var itemModel = inventoryItemsModel.SlotsTargetItemModels[targetSlotIndex];

                if (itemModel != null)
                    return;

                window.InventoryItemsBlockSerializableComponents[targetSlotIndex].ItemImage.gameObject.SetActive(false);
                window.InventoryItemsBlockSerializableComponents[targetSlotIndex].ItemsCountText.gameObject.SetActive(false);
            }
        }
    }
}