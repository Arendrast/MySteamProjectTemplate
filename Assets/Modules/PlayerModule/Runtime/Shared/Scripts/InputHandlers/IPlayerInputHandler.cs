using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public interface IExitablePlayerInputHandler : IPlayerInputHandler
    {
        void Exit();
    }
    
    public interface IPlayerInputHandler : IOwnerPlayerComponent
    {
        void Update();
        PlayerInputHandlerType GetInputHandlerType();
    }
}