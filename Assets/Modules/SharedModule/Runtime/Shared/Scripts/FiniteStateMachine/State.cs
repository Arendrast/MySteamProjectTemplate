using System;

namespace Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine
{
    public abstract class State : IState
    {
        public bool IsStateEnded { get; private set; }
        public bool IsActive { get; private set; }
        public event Action Entered, Exited, Updated, FixedUpdated, LateUpdated;

        public void Update(float time)
        {
            OnUpdate(time);
            Updated?.Invoke();
        }

        public void LateUpdate(float time)
        {
            OnLateUpdate(time);
            LateUpdated?.Invoke();
        }

        public void FixedUpdate(float time)
        {
            OnFixedUpdate(time);
            FixedUpdated?.Invoke();
        }

        public void Enter(IState nextState)
        {
            //Debug.Log($"Enter {GetType().Name}");
            OnEnter(nextState);
            IsStateEnded = false;
            IsActive = true;
            Entered?.Invoke();
        }

        public void Exit(IState pastState)
        {
            OnExit(pastState);
            IsStateEnded = false;
            IsActive = false;
            Exited?.Invoke();
        }

        protected virtual void OnEnter(IState pastState) { }
        protected virtual void OnExit(IState nextState) { }
        protected virtual void OnUpdate(float time) { }
        protected virtual void OnLateUpdate(float time) {}
        protected virtual void OnFixedUpdate(float time) { }
    }
}
