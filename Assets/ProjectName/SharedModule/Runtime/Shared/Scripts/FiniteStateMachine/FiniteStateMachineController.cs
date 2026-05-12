using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine
{
    public class FiniteStateMachineController<TState> where TState : class, IState
    {
        public readonly FiniteStateMachineModel<TState> Model;

        public FiniteStateMachineController(FiniteStateMachineModel<TState> model, MonoBehaviourObserver monoBehaviourObserver)
        {
            Model = model;
            monoBehaviourObserver.Updated += ChangeCurrentNodeState;
            monoBehaviourObserver.Updated += UpdateCurrentNodeState;
            monoBehaviourObserver.FixedUpdated += FixedUpdateCurrentNodeState;
            monoBehaviourObserver.LateUpdated += LateUpdateCurrentNodeState;
        }

        private void ChangeCurrentNodeState()
        {
            Model.TryChangingStateByCurrentTransitionRecursively();
        }

        private void UpdateCurrentNodeState()
        {
            Model.CurrentNode?.State?.Update();
        }
        
        private void FixedUpdateCurrentNodeState()
        {
            Model.CurrentNode?.State?.FixedUpdate();
        }

        private void LateUpdateCurrentNodeState()
        {
            Model.CurrentNode?.State?.LateUpdate();
        }
    }
}