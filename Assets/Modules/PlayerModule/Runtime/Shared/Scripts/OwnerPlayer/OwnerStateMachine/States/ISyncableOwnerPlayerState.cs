using Modules.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States
{
    public interface ISyncableOwnerPlayerState
    {
        SharedPlayerStateType GetSharedStateType();
    }
}