using System;

namespace Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine
{
    public interface IState
    {
        bool IsStateEnded { get; }
        bool IsActive{ get; }
        event Action Entered, Exited, Updated, FixedUpdated, LateUpdated;
        void Update();
        void LateUpdate();
        void FixedUpdate();
        void Enter(IState pastState);
        void Exit(IState nextState);
    }
}