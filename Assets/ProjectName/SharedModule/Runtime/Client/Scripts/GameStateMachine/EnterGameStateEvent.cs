using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using ProjectName.SharedModule.Runtime.Shared.Scripts.GameStateMachine;

namespace ProjectName.SharedModule.Runtime.Client.Scripts.GameStateMachine
{
    public struct EnterGameStateEvent : IEvent
    {
        public readonly GameStateType GameStateType;
        public readonly IGameStateEnterData GameStateEnterData;

        public EnterGameStateEvent(GameStateType gameStateType, IGameStateEnterData gameStateEnterData = null)
        {
            GameStateType = gameStateType;
            GameStateEnterData = gameStateEnterData;
        }
    }
}