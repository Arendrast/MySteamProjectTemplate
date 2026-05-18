using System.Linq;
using Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers;
using Modules.PlayerModule.Runtime.Shared.Scripts.Movement;
using Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates
{
    public class GroundIdleState : State, IFeetOwnerPlayerState
    {
        private readonly PlayerMovementStateController _movementStateController;
        private readonly AllInputHandlersHandler _allInputHandlersHandler;
        private readonly PlayerInputHandlerType[] _handlersTypes;

        public GroundIdleState(PlayerMovementStateController movementStateController,
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
            _allInputHandlersHandler.SubscribeNewInputHandlers(_handlersTypes);
        }

        protected override void OnUpdate(float time)
        {
            _movementStateController.UpdateAndApplyMovement(true, time);
        }
    }
}