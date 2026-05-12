using FishNet.Broadcast;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.Network
{
    public struct UpdatePlayerSharedStateBroadcastForServer : IBroadcast
    {
        public readonly SharedPlayerStateType StateType;
        
        public UpdatePlayerSharedStateBroadcastForServer(SharedPlayerStateType stateType)
        {
            StateType = stateType;
        }
    }
}