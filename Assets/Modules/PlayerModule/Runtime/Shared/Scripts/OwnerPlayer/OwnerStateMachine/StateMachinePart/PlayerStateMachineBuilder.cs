using System;
using System.Collections.Generic;
using System.Linq;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using VContainer;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.StateMachinePart
{
    public class PlayerStateMachineBuilder<TState> where TState : class, IOwnerPlayerState
    {
        private readonly FiniteStateMachineModel<TState> _model;
        private readonly IObjectResolver _resolver;

        public PlayerStateMachineBuilder(FiniteStateMachineModel<TState> model, IObjectResolver resolver)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public FromBuilder<TFrom> From<TFrom>() where TFrom : class, TState
        {
            return new FromBuilder<TFrom>(this, ResolveState<TFrom>());
        }

        public FromManyBuilder FromAny()
        {
            return new FromManyBuilder(this, ResolveAll<TState>().ToArray());
        }

        public FromManyBuilder FromMany(TState[] fromStates)
        {
            return new FromManyBuilder(this, fromStates);
        }

        public ToManyBuilder FromManyTo<TTo>(TState[] fromStates) where TTo : class, TState
        {
            return FromMany(fromStates).To<TTo>();
        }

        public void Build()
        {
            _model.Initialize();
        }

        private void AddTransition<TFrom, TTo>(TFrom from, TTo to, Func<bool> predicate,
            Func<bool> reversePredicate = null)
            where TFrom : TState
            where TTo : TState
        {
            _model.AddTransition(from, to, predicate, reversePredicate);
        }

        private void AddTransition(IEnumerable<TState> fromEnumerable, TState to,
            Func<bool> predicate)
        {
            _model.AddTransition(fromEnumerable, to, predicate);
        }

        private TLocalState ResolveState<TLocalState>() where TLocalState : class, TState
        {
            var resolved = _resolver.Resolve<TLocalState>();
            if (resolved == null)
                throw new InvalidOperationException(
                    $"Resolve<{typeof(TLocalState).Name}>() returned null. Check DI registration.");
            return resolved;
        }

        private IEnumerable<TState> ResolveAll<T>() where T : class, TState
        {
            var resolved = _resolver.Resolve<IEnumerable<T>>();
            if (resolved == null)
                return Enumerable.Empty<TState>();
            return resolved.Cast<TState>();
        }

        public class FromBuilder<TFrom> where TFrom : class, TState
        {
            private readonly PlayerStateMachineBuilder<TState> _parent;
            private readonly TFrom _fromState;

            internal FromBuilder(PlayerStateMachineBuilder<TState> parent, TFrom fromState)
            {
                _parent = parent;
                _fromState = fromState ?? throw new ArgumentNullException(nameof(fromState));
            }

            public ToBuilder<TFrom, TTo> To<TTo>() where TTo : class, TState
            {
                return new ToBuilder<TFrom, TTo>(_parent, _fromState, _parent.ResolveState<TTo>());
            }
        }

        public class FromManyBuilder
        {
            private readonly PlayerStateMachineBuilder<TState> _parent;
            private TState[] _fromStates;

            internal FromManyBuilder(PlayerStateMachineBuilder<TState> parent, TState[] fromStates)
            {
                _parent = parent ?? throw new ArgumentNullException(nameof(parent));
                _fromStates = fromStates ?? throw new ArgumentNullException(nameof(fromStates));
            }

            public FromManyBuilder Without<T>() where T : class, TState
            {
                var excludeType = typeof(T);
                _fromStates = _fromStates.Where(s => s.GetType() != excludeType).ToArray();
                return this;
            }

            public ToManyBuilder To<TTo>() where TTo : class, TState
            {
                if (_fromStates.Length == 0)
                    throw new InvalidOperationException("No source states remain after applying Without<T>() filters.");

                return new ToManyBuilder(_parent, _fromStates, new []{_parent.ResolveState<TTo>()});
            }
            
            public ToManyBuilder ToMany(TState[] states)
            {
                if (_fromStates.Length == 0)
                    throw new InvalidOperationException("No source states remain after applying Without<T>() filters.");

                return new ToManyBuilder(_parent, _fromStates, states);
            }
        }

        public class ToBuilder<TFrom, TTo>
            where TFrom : TState
            where TTo : TState
        {
            private readonly PlayerStateMachineBuilder<TState> _parent;
            private readonly TFrom _from;
            private readonly TTo _to;

            internal ToBuilder(PlayerStateMachineBuilder<TState> parent, TFrom from, TTo to)
            {
                _parent = parent;
                _from = from ?? throw new ArgumentNullException(nameof(from));
                _to = to ?? throw new ArgumentNullException(nameof(to));
            }

            public void When(Func<bool> predicateFactory)
            {
                if (predicateFactory == null) throw new ArgumentNullException(nameof(predicateFactory));
                _parent.AddTransition(_from, _to, predicateFactory);
            }

            public void WhenNever()
            {
                When(() => false);
            }

            public void WhenAndAndGoBackWhenReverse(Func<bool> predicate)
            {
                if (predicate == null) throw new ArgumentNullException(nameof(predicate));
                _parent.AddTransition(_from,
                    _to,
                    predicate,
                    GetReversedPredicate);

                bool GetReversedPredicate()
                {
                    return !predicate.Invoke();
                }
            }

            public void When(Func<bool> predicateFactory,
                Func<bool> reversePredicateFactory)
            {
                if (predicateFactory == null) throw new ArgumentNullException(nameof(predicateFactory));
                if (reversePredicateFactory == null) throw new ArgumentNullException(nameof(reversePredicateFactory));
                _parent.AddTransition(_from,
                    _to,
                    predicateFactory,
                    reversePredicateFactory);
            }
        }

        public class ToManyBuilder
        {
            private readonly PlayerStateMachineBuilder<TState> _parent;
            private readonly TState[] _fromStates;
            private readonly TState[] _to;

            internal ToManyBuilder(PlayerStateMachineBuilder<TState> parent, TState[] fromStates,
                TState[] to)
            {
                _parent = parent;
                FromStatesCheck(fromStates);
                _fromStates = fromStates ?? throw new ArgumentNullException(nameof(fromStates));
                _to = to ?? throw new ArgumentNullException(nameof(to));
            }

            private static void FromStatesCheck(TState[] fromStates)
            {
                if (fromStates == null) throw new ArgumentNullException(nameof(fromStates));
            }

            public ToManyBuilder When(Func<bool> predicateFactory)
            {
                if (predicateFactory == null) throw new ArgumentNullException(nameof(predicateFactory));

                foreach (var state in _to)
                {
                    _parent.AddTransition(_fromStates, state, predicateFactory);   
                }
                return this;
            }

            public ToManyBuilder WhenNever()
            {
                return When(() => false);
            }

            public PlayerStateMachineBuilder<TState> WhenAndBackWhenInverseTo<TState2>(
                Func<bool> predicateFactory)  where TState2 : class, TState
            {
                var when = When(predicateFactory.Invoke);

                return when.AndBackWhenTo<TState2>(GetInversedPredicate);

                bool GetInversedPredicate()
                {
                    return !predicateFactory.Invoke();
                }
            }

            public PlayerStateMachineBuilder<TState> AndBackWhenTo<TBack>(Func<bool> reversePredicateFactory)
                where TBack : class, TState
            {
                if (reversePredicateFactory == null) throw new ArgumentNullException(nameof(reversePredicateFactory));
                var backState = _parent.ResolveState<TBack>();
                _parent.AddTransition(_to, backState, reversePredicateFactory);
                return _parent;
            }
        }
    }
}