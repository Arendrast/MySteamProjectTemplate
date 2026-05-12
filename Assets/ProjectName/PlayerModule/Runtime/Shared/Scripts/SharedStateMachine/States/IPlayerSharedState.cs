using ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States
{
    public interface IPlayerSharedState : IState
    {
        SharedPlayerStateType GetStateType();
    }
}