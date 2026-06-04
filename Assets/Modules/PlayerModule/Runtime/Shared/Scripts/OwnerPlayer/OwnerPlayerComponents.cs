using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Push;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers;
using Modules.PlayerModule.Runtime.Shared.Scripts.Interaction;
using Modules.PlayerModule.Runtime.Shared.Scripts.Movement;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.HandsStates;
using Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer
{
    public class OwnerPlayerComponents
    {
        public readonly ClientPlayerComponents ClientComponents;
        public readonly OwnerPlayerSerializableComponents SerializableComponents;

        public readonly FiniteStateMachineModel<IHandsOwnerPlayerState> HandsStateMachineModel;
        public readonly FiniteStateMachineModel<IFeetOwnerPlayerState> FeetStateMachineModel;

        public readonly ActiveInputHandlersTypesRepository ActiveInputHandlersTypesRepository;
        public readonly PlayerInteractionController InteractionController;
        public readonly PlayerMovementController MovementController;
        public readonly PlayerIsGroundedProvider IsGroundedProvider;
        public readonly PushHandlerController PushHandlerController;
        public readonly PushHandlerModel PushHandlerModel;
        public readonly DamageReceiversFinder DamageReceiversFinder;


        public OwnerPlayerComponents(
            ClientPlayerComponents clientComponents,
            OwnerPlayerSerializableComponents serializableComponents,
            FiniteStateMachineModel<IHandsOwnerPlayerState> handsStateMachineModel,
            FiniteStateMachineModel<IFeetOwnerPlayerState> feetStateMachineModel,
            PlayerInteractionController interactionController,
            PlayerMovementController movementController,
            PushHandlerController pushHandlerController,
            PushHandlerModel pushHandlerModel,
            DamageReceiversFinder damageReceiversFinder,
            PlayerIsGroundedProvider isGroundedProvider, 
            ActiveInputHandlersTypesRepository activeInputHandlersTypesRepository)
        {
            ClientComponents = clientComponents;
            SerializableComponents = serializableComponents;
            HandsStateMachineModel = handsStateMachineModel;
            InteractionController = interactionController;
            MovementController = movementController;
            DamageReceiversFinder = damageReceiversFinder;
            PushHandlerController = pushHandlerController;
            PushHandlerModel = pushHandlerModel;
            IsGroundedProvider = isGroundedProvider;
            ActiveInputHandlersTypesRepository = activeInputHandlersTypesRepository;
            FeetStateMachineModel = feetStateMachineModel;
        }
    }
}