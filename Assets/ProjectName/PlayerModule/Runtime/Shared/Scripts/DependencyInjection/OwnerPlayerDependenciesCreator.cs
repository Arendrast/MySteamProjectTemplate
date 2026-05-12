using ProjectName.PlayerModule.Runtime.Shared.Scripts.InputHandlers;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.HandsStates;
using ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using VContainer.Unity;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.DependencyInjection
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