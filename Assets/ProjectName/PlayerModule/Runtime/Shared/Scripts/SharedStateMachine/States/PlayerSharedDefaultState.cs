using ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States
{
    public class PlayerSharedDefaultState : State, IPlayerSharedState
    {
        public SharedPlayerStateType GetStateType()
        {
            return SharedPlayerStateType.Default;
        }
    }
}