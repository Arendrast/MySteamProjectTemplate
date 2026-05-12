using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.Reactors
{
    public interface IEnterableExitableOwnerPlayerStateReactor : IOwnerPlayerStateReactor
    {
        void OnExitState(IOwnerPlayerState pastPlayerState, IOwnerPlayerState newPlayerState);
        void OnEnterState(IOwnerPlayerState pastPlayerState, IOwnerPlayerState newPlayerState);
    }
}