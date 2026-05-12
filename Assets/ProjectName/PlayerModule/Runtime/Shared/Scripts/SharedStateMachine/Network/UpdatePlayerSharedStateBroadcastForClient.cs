using FishNet.Broadcast;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.Network
{
    public struct UpdatePlayerSharedStateBroadcastForClient : IBroadcast
    {
        public readonly SharedPlayerStateType StateType;
        public readonly int ClientId;

        public UpdatePlayerSharedStateBroadcastForClient(SharedPlayerStateType stateType, int clientId)
        {
            StateType = stateType;
            ClientId = clientId;
        }
    }
}