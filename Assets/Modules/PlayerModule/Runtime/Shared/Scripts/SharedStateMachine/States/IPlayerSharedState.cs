using Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States
{
    public interface IPlayerSharedState : IState
    {
        SharedPlayerStateType GetStateType();
    }
}