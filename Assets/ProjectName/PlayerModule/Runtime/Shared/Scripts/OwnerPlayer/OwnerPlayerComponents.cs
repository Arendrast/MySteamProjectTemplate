using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Push;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.InputHandlers;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.Interaction;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.Movement;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.HandsStates;
using ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer
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
        public readonly CharacterControllerPushHandlerController PushHandlerController;
        public readonly CharacterControllerPushHandlerModel PushHandlerModel;
        public readonly DamageReceiversFinder DamageReceiversFinder;


        public OwnerPlayerComponents(
            ClientPlayerComponents clientComponents,
            OwnerPlayerSerializableComponents serializableComponents,
            FiniteStateMachineModel<IHandsOwnerPlayerState> handsStateMachineModel,
            FiniteStateMachineModel<IFeetOwnerPlayerState> feetStateMachineModel,
            PlayerInteractionController interactionController,
            PlayerMovementController movementController,
            CharacterControllerPushHandlerController pushHandlerController,
            CharacterControllerPushHandlerModel pushHandlerModel,
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