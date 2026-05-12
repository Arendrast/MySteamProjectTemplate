using System.Linq;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public static class PlayerInputHandlersTools
    {
        private static readonly PlayerInputHandlerType[] _inventoryPartInputHandlers = new PlayerInputHandlerType[]
        {
            PlayerInputHandlerType.InventoryItemsTargetSlotChange,
        };

        public static PlayerInputHandlerType[] GetPlayerInputHandlerTypes(bool shouldIncludeAllInventoryPartHandlers = false,
            params PlayerInputHandlerType[] types)
        {
            if (shouldIncludeAllInventoryPartHandlers)
            {
                types = types.Concat(_inventoryPartInputHandlers).ToArray();
            }

            return types.Distinct().ToArray();
        }
    }
}