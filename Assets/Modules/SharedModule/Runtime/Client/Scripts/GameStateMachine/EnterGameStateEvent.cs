using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.GameStateMachine;

namespace Modules.SharedModule.Runtime.Client.Scripts.GameStateMachine
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