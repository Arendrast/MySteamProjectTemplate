using System;

namespace Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine
{
    public interface IState
    {
        bool IsStateEnded { get; }
        bool IsActive{ get; }
        event Action Entered, Exited, Updated, FixedUpdated, LateUpdated;
        void Update(float time);
        void LateUpdate(float time);
        void FixedUpdate(float time);
        void Enter(IState pastState);
        void Exit(IState nextState);
    }
}