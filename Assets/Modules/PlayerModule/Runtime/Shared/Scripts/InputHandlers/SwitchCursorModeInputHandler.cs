using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class SwitchCursorModeInputHandler : IPlayerInputHandler
    {
        private readonly IInputProvider _inputProvider;
        private readonly PlayerInputHandler _playerInputHandler;

        public SwitchCursorModeInputHandler(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
            _playerInputHandler = new PlayerInputHandler(GetInputCondition, TrySwitchCursor);
        }

        public void Update()
        {
            _playerInputHandler.InvokeActions();
        }

        public PlayerInputHandlerType GetInputHandlerType()
        {
            return PlayerInputHandlerType.SwitchCursorMode;
        }

        private bool GetInputCondition()
        {
            return _inputProvider.IsActionTriggered(InputActionType.SetCursorMode);
        }

        private void TrySwitchCursor()
        {
            CursorSwitchTools.TrySwitchCursor();
        }
    }
}