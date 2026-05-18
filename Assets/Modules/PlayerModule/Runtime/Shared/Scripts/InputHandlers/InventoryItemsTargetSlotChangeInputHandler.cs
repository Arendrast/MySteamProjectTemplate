using Modules.InventoryModule.Runtime.Shared.Scripts;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using UnityEngine.InputSystem;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class InventoryItemsTargetSlotChangeInputHandler : IPlayerInputHandler
    {
        private int? _triggeredSelectItemIndex;
        private readonly InventoryItemsModel _inventoryItemsModel;
        private readonly IInputService _inputService;
        private readonly InventoryItemsConfig _inventoryItemsConfig;

        private InventoryItemsTargetSlotChangeInputHandler(
            InventoryItemsModel inventoryItemsModel, IInputService inputService,
            InventoryItemsConfig inventoryItemsConfig)
        {
            _inventoryItemsModel = inventoryItemsModel;
            _inputService = inputService;
            _inventoryItemsConfig = inventoryItemsConfig;
        }

        public void SetSubscribeState(SubscribeState subscribeState)
        {
            _inputService.SetSubscribeStateToInputActionGroup(InputActionGroupType.SelectItem, InputActionPhase.Started,
                TryStartToSetTargetSlot, _inventoryItemsConfig.ItemSlotsAmount, subscribeState);
        }

        public PlayerInputHandlerType GetInputHandlerType()
        {
            return PlayerInputHandlerType.InventoryItemsTargetSlotChange;
        }

        private void TryStartToSetTargetSlot(InputAction.CallbackContext callbackContext, int itemIndex) =>
            _inventoryItemsModel.TryStartToSetTargetSlot(itemIndex);
    }
}