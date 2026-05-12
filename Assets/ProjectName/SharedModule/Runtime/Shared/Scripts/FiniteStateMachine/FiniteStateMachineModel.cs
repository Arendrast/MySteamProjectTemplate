using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine
{
    public class FiniteStateMachineModel<TState> where TState : class, IState
    {
        public IReadOnlyDictionary<Type, StateNode<TState>> Nodes => _nodes;
        public StateNode<TState> CurrentNode { get; private set; }
        public event Action<TState> UpdatedState, FixedUpdatedState;
        public event Action<TState, TState> EnteredState, ChangedState, ExitedState;

        private const int MaxTransitionsCount = 20;

        private readonly Dictionary<Type, StateNode<TState>> _nodes = new();

        public void AddTransition(TState from, TState to, IPredicate predicate, IPredicate reversePredicate = null)
        {
            TryGetOrAddNode(from).AddTransition(TryGetOrAddNode(to).State, predicate);
            if (reversePredicate != null)
            {
                TryGetOrAddNode(to).AddTransition(TryGetOrAddNode(from).State, reversePredicate);
            }
        }

        public void EnterState(TState state)
        {
            TryChangeState(state, false);
            TryChangingStateByCurrentTransitionRecursively();
        }

        public bool TryEnterState<TLocalState>(bool onlyCheck = false) where TLocalState : TState
        {
            if (CurrentNode.Transitions.Count == 0)
                return false;

            var transition = CurrentNode.Transitions.FirstOrDefault(transition => transition.ToState is TLocalState);

            if (transition != null)
            {
                if (!onlyCheck)
                {
                    EnterState(transition.ToState);
                }

                return true;
            }

            return false;
        }

        public void AddTransition(TState from, TState to, Func<bool> predicate, Func<bool> reversePredicate = null)
        {
            AddTransition(from, to,
                new FuncPredicate(predicate),
                reversePredicate != null ? new FuncPredicate(reversePredicate) : null);
        }

        public void AddTransition(IEnumerable<TState> fromEnumerable, TState to, IPredicate predicate)
        {
            fromEnumerable.ForEach(from =>
                TryGetOrAddNode(from).AddTransition(TryGetOrAddNode(to).State, predicate));
        }

        public void TryAddState(TState state)
        {
            TryGetOrAddNode(state);
        }

        public void AddTransition(IEnumerable<TState> fromEnumerable, TState to, Func<bool> predicate)
        {
            AddTransition(fromEnumerable, to, new FuncPredicate(predicate));
        }

        public Transition<TState> GetTransition()
        {
            return CurrentNode.Transitions.FirstOrDefault(transition => transition.Condition.Evaluate());
        }

        public void AddTransitionToSelf(TState from, IPredicate predicate)
        {
            AddTransition(from, from, predicate);
        }

        public void AddTransitionToSelf(TState from, Func<bool> predicate)
        {
            AddTransition(from, from, new FuncPredicate(predicate));
        }

        public void Initialize()
        {
            CurrentNode.State.Enter(null);
        }

        public bool TryChangeState(TState state, bool onlyCheck, TState pastState = null)
        {
            //Debug.Log($"Enter {state}");
            return TryChangeState(TryGetOrAddNode(state)?.State.GetType(), onlyCheck, pastState);
        }

        public void TryChangingStateByCurrentTransitionRecursively()
        {
            if (Nodes.Count == 0)
            {
                return;
            }

            var count = 0;

            while (GetTransition() is { } transition)
            {
                TryChangeState(transition.ToState, false);
                count++;

                if (count < MaxTransitionsCount) continue;
                Debug.LogError($"Deadlock transition state machine to state {transition.ToState.GetType().Name}");
                return;
            }
        }

        private StateNode<TState> TryGetOrAddNode(TState state)
        {
            if (state == null)
                return null;

            var node = _nodes.GetValueOrDefault(state.GetType());

            if (node != null) return node;

            node = new StateNode<TState>(state);
            _nodes.Add(state.GetType(), node);

            CurrentNode ??= node;

            return node;
        }

        private bool TryChangeState(Type stateType, bool onlyCheck = false, TState pastState = null)
        {
            if (stateType == null || !_nodes.TryGetValue(stateType, out var nextNode))
                return false;

            if (onlyCheck)
                return true;

            pastState ??= CurrentNode?.State;
            var newState = nextNode?.State;

            TryExitCurrentState(newState);

            CurrentNode = nextNode;

            if (newState != null)
            {
                CurrentNode.State.Updated += InvokeUpdatedState;
                CurrentNode.State.FixedUpdated += InvokeFixedUpdatedState;
                CurrentNode.State.Enter(pastState);
                EnteredState?.Invoke(pastState, newState);
            }

            if (pastState != null && newState != null)
            {
                ChangedState?.Invoke(pastState, newState);
            }

            return true;
        }

        private void InvokeUpdatedState()
        {
            UpdatedState?.Invoke(CurrentNode.State);
        }

        private void InvokeFixedUpdatedState()
        {
            FixedUpdatedState?.Invoke(CurrentNode.State);
        }

        public void TryExitCurrentState(TState newState)
        {
            var pastState = CurrentNode?.State;

            if (pastState != null)
            {
                CurrentNode.State.Updated -= InvokeUpdatedState;
                CurrentNode.State.FixedUpdated -= InvokeFixedUpdatedState;
                CurrentNode.State.Exit(newState);
                ExitedState?.Invoke(pastState, newState);
            }
            
            CurrentNode = null;
        }
    }
}