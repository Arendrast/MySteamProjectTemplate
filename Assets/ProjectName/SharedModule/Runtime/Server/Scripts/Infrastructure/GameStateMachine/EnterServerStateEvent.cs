using FishNet.Connection;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;

namespace ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure.GameStateMachine
{
    public struct EnterServerStateEvent : IEvent
    {
        public readonly ServerGameStateType GameStateType;
        public readonly NetworkConnection Connection;

        public EnterServerStateEvent(ServerGameStateType gameStateType, NetworkConnection connection)
        {
            GameStateType = gameStateType;
            Connection = connection;
        }
    }
}