using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine
{
    public class FiniteStateMachineController<TState> where TState : class, IState
    {
        public readonly FiniteStateMachineModel<TState> Model;

        public FiniteStateMachineController(FiniteStateMachineModel<TState> model, GameObject gameObject,
            UpdateObserversService updateObserversService)
        {
            updateObserversService.TryAddOrGetUpdateObserver(gameObject, UpdateType.Update, out var updateObserver);
            updateObserversService.TryAddOrGetUpdateObserver(gameObject, UpdateType.LateUpdate, out var lateUpdateObserver);
            updateObserversService.TryAddOrGetUpdateObserver(gameObject, UpdateType.FixedUpdate, out var fixedUpdateObserver);


            Model = model;
            updateObserver.Updated += ChangeCurrentNodeState;
            updateObserver.Updated += UpdateCurrentNodeState;
            lateUpdateObserver.Updated += LateUpdateCurrentNodeState;
            fixedUpdateObserver.Updated += FixedUpdateCurrentNodeState;
        }

        private void ChangeCurrentNodeState(float time)
        {
            Model.TryChangingStateByCurrentTransitionRecursively();
        }

        private void UpdateCurrentNodeState(float time)
        {
            Model.CurrentNode?.State?.Update(time);
        }

        private void FixedUpdateCurrentNodeState(float time)
        {
            Model.CurrentNode?.State?.FixedUpdate(time);
        }

        private void LateUpdateCurrentNodeState(float time)
        {
            Model.CurrentNode?.State?.LateUpdate(time);
        }
    }
}