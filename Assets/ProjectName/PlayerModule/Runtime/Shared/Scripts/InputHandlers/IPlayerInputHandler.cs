using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.InputHandlers
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