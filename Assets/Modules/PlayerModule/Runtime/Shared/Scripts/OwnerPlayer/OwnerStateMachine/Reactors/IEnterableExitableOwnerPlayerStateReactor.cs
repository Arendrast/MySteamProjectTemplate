using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.Reactors
{
    public interface IEnterableExitableOwnerPlayerStateReactor : IOwnerPlayerStateReactor
    {
        void OnExitState(IOwnerPlayerState pastPlayerState, IOwnerPlayerState newPlayerState);
        void OnEnterState(IOwnerPlayerState pastPlayerState, IOwnerPlayerState newPlayerState);
    }
}