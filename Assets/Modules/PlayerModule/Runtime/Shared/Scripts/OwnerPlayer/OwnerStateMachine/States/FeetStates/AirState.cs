using Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers;
using Modules.PlayerModule.Runtime.Shared.Scripts.Movement;
using Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates
{
    public class AirState : State, IFeetOwnerPlayerState
    {
        private readonly PlayerMovementStateController _movementStateController;
        private readonly AllInputHandlersHandler _allInputHandlersHandler;

        private readonly PlayerInputHandlerType[] _handlersTypes;

        public AirState(PlayerMovementStateController movementStateController,
            AllInputHandlersHandler allInputHandlersHandler)
        {
            _movementStateController = movementStateController;
            _allInputHandlersHandler = allInputHandlersHandler;

            _handlersTypes = PlayerInputHandlersTools.GetPlayerInputHandlerTypes(true);
        }

        protected override void OnEnter(IState pastState)
        {
        }

        protected override void OnExit(IState nextState)
        {
        }

        protected override void OnUpdate()
        {
            _allInputHandlersHandler.TryUpdateSelectedHandlers(_handlersTypes);
            _movementStateController.UpdateAndApplyMovement(true);
        }
    }
}