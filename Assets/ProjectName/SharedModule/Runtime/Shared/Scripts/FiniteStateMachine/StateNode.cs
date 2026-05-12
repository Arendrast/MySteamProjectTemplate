using System.Collections.Generic;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine
{
    public class StateNode<TState> where TState : class, IState
    {
        public IReadOnlyCollection<Transition<TState>> Transitions => _transitions;
        public readonly TState State;
        private readonly HashSet<Transition<TState>> _transitions = new HashSet<Transition<TState>>();


        public StateNode(TState state) => State = state;

        public void AddTransition(TState to, IPredicate condition) => 
            _transitions.Add(new Transition<TState>(to, condition));
    }
}