using Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.HandsStates;
using Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using VContainer.Unity;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.DependencyInjection
{
    public class OwnerPlayerDependenciesCreator : IStartable
    {
        public OwnerPlayerDependenciesCreator(
            InventoryItemsTargetSlotChangeInputHandler inventoryItemsTargetSlotChangeInputHandler,
            FiniteStateMachineController<IHandsOwnerPlayerState> handsStateMachineController,
            FiniteStateMachineController<IFeetOwnerPlayerState> feetStateMachineController,
            PlayerInteractionInputHandler interactionInputHandler)
        {
        }

        public void Start()
        {
        }
    }
}