using FishNet.Broadcast;
using Modules.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.Network
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