using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using InputActionType = Modules.SharedModule.Runtime.Shared.Scripts.Input.InputActionType;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class SwitchCursorModeInputHandler : IPlayerInputHandler
    {
        private readonly IInputService _inputService;

        public SwitchCursorModeInputHandler(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void SetSubscribeState(SubscribeState subscribeState)
        {
            _inputService.SetSubscribeStateToInputAction(InputActionType.SetCursorMode, InputActionPhase.Started, TrySwitchCursor, subscribeState);
        }

        public PlayerInputHandlerType GetInputHandlerType()
        {
            return PlayerInputHandlerType.SwitchCursorMode;
        }

        private void TrySwitchCursor(InputAction.CallbackContext callbackContext)
        {
            CursorSwitchTools.TrySwitchCursor();
        }
    }
}