using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public interface IPlayerInputHandler : IOwnerPlayerComponent
    {
        void SetSubscribeState(SubscribeState subscribeState);
        PlayerInputHandlerType GetInputHandlerType();
    }
}