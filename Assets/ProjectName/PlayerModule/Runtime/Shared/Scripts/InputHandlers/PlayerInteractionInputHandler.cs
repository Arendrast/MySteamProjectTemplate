using ProjectName.PlayerModule.Runtime.Shared.Scripts.Interaction;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Input;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class PlayerInteractionInputHandler : IPlayerInputHandler
    {
        private readonly IInputProvider _inputProvider;
        private readonly PlayerInteractionController _interactionController;

        public PlayerInteractionInputHandler(
            IInputProvider inputProvider,
            PlayerInteractionController interactionController)
        {
            _inputProvider = inputProvider;
            _interactionController = interactionController;
        }

        public void Update()
        {
            _interactionController.TryInteractAsync(GetInputCondition());
        }

        public PlayerInputHandlerType GetInputHandlerType()
        {
            return PlayerInputHandlerType.Interaction;
        }

        private bool GetInputCondition()
        {
            return _inputProvider.IsActionTriggered(InputActionType.Use);
        }
    }
}