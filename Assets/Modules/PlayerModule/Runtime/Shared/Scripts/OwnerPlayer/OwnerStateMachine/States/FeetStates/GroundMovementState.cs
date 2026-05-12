using System.Linq;
using Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers;
using Modules.PlayerModule.Runtime.Shared.Scripts.Movement;
using Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates
{
    public class GroundMovementState : State, IFeetOwnerPlayerState
    {
        private readonly PlayerMovementStateController _movementStateController;
        private readonly AllInputHandlersHandler _allInputHandlersHandler;
        private readonly PlayerInputHandlerType[] _handlersTypes;

        public GroundMovementState(PlayerMovementStateController movementStateController,
            AllInputHandlersHandler allInputHandlersHandler)
        {
            _movementStateController = movementStateController;
            _allInputHandlersHandler = allInputHandlersHandler;
            _handlersTypes =
                PlayerInputHandlersTools.GetPlayerInputHandlerTypes(true,
                    CollectionTools.ParseEnumToList<PlayerInputHandlerType>().Except(new[]
                        {
                            PlayerInputHandlerType.None
                        })
                        .ToArray());
        }

        protected override void OnEnter(IState pastState)
        {
        }

        protected override void OnUpdate()
        {
            _allInputHandlersHandler.TryUpdateSelectedHandlers(_handlersTypes);
            _movementStateController.UpdateAndApplyMovement(true);
        }
    }
}