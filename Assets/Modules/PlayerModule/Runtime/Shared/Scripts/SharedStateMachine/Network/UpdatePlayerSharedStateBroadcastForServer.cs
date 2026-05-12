using FishNet.Broadcast;
using Modules.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.Network
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