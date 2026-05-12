using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.Reactors
{
    public interface IOwnerPlayerStateReactor : IOwnerPlayerComponent
    {
        void OnChangeState(IOwnerPlayerState pastPlayerState, IOwnerPlayerState newPlayerState);
        void OnUpdate(IOwnerPlayerState ownerPlayerState);
    }
}