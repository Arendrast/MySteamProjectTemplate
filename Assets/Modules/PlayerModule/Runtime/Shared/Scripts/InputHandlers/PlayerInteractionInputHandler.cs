using Modules.PlayerModule.Runtime.Shared.Scripts.Interaction;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using UnityEngine.InputSystem;
using InputActionType = Modules.SharedModule.Runtime.Shared.Scripts.Input.InputActionType;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class PlayerInteractionInputHandler : IPlayerInputHandler
    {
        private readonly IInputService _inputService;
        private readonly PlayerInteractionController _interactionController;

        public PlayerInteractionInputHandler(
            IInputService inputService,
            PlayerInteractionController interactionController)
        {
            _inputService = inputService;
            _interactionController = interactionController;
        }

        public PlayerInputHandlerType GetInputHandlerType()
        {
            return PlayerInputHandlerType.Interaction;
        }

        public void SetSubscribeState(SubscribeState subscribeState)
        {
            _inputService.SetSubscribeStateToInputAction(InputActionType.Use, InputActionPhase.Started,
                TryInteractAsync, subscribeState);
        }

        private void TryInteractAsync(InputAction.CallbackContext callbackContext)
        {
            _interactionController.TryInteractAsync();
        }
    }
}