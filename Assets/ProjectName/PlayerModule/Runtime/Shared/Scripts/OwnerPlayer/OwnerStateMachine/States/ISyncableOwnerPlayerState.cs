using ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States
{
    public interface ISyncableOwnerPlayerState
    {
        SharedPlayerStateType GetSharedStateType();
    }
}