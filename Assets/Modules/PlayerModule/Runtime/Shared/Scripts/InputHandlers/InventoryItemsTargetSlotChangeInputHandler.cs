using Modules.InventoryModule.Runtime.Shared.Scripts;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class InventoryItemsTargetSlotChangeInputHandler : IPlayerInputHandler
    {
        private int? _triggeredSelectItemIndex;
        private readonly InventoryItemsModel _inventoryItemsModel;
        private readonly IInputProvider _inputProvider;
        private readonly InventoryItemsConfig _inventoryItemsConfig;
        private readonly PlayerInputHandler _playerInputHandler;

        private InventoryItemsTargetSlotChangeInputHandler(
            InventoryItemsModel inventoryItemsModel, IInputProvider inputProvider,
            InventoryItemsConfig inventoryItemsConfig)
        {
            _inventoryItemsModel = inventoryItemsModel;
            _inputProvider = inputProvider;
            _inventoryItemsConfig = inventoryItemsConfig;
            _playerInputHandler = new PlayerInputHandler(GetInputCondition, TryStartToSetTargetSlot);
        }

        public void Update()
        {
            _playerInputHandler.InvokeActions();
        }

        public PlayerInputHandlerType GetInputHandlerType()
        {
            return PlayerInputHandlerType.InventoryItemsTargetSlotChange;
        }

        private bool GetInputCondition() =>
            (_triggeredSelectItemIndex =
                _inputProvider.GetTriggeredSelectItemIndex(_inventoryItemsConfig.ItemSlotsAmount)).HasValue;

        private void TryStartToSetTargetSlot() =>
            _inventoryItemsModel.TryStartToSetTargetSlot(_triggeredSelectItemIndex.Value);
    }
}