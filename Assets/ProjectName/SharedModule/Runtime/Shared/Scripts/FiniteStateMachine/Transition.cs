namespace ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine
{
    public class Transition<TState> : ITransition where TState : IState
    {
        public TState ToState { get; }
        public IPredicate Condition { get; }

        public Transition(TState toState, IPredicate condition)
        {
            ToState = toState;
            Condition = condition;
        }
    }
}