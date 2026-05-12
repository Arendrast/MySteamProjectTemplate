using System;
using System.Linq;
using FishNet.Managing.Client;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.HandsStates;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.Network;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;
using ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.Reactors
{
    public class OwnerPlayerStateSharedReactor : IEnterableExitableOwnerPlayerStateReactor, IDisposable
    {
        private SharedPlayerStateType? _stateTypeForSend;
        
        private readonly FiniteStateMachineModel<IPlayerSharedState> _sharedStateMachine;
        private readonly DeferredActionInvoker _deferredActionInvoker;

        public OwnerPlayerStateSharedReactor(ClientManager clientManager,
            FiniteStateMachineModel<IPlayerSharedState> sharedStateMachine,
            CustomCoroutineRunner customCoroutineRunner)
        {
            _sharedStateMachine = sharedStateMachine;

            _deferredActionInvoker = new DeferredActionInvoker(customCoroutineRunner);
            _deferredActionInvoker.InvokedOnEndFrame += TrySendUpdateStateBroadcast;

            return;

            void TrySendUpdateStateBroadcast()
            {
                if (!_stateTypeForSend.HasValue) return;
                
                clientManager.Broadcast(new UpdatePlayerSharedStateBroadcastForServer(_stateTypeForSend.Value));
                _stateTypeForSend = null;
            }
        }

        public void Dispose()
        {
            _deferredActionInvoker.Dispose();
        }

        public void OnChangeState(IOwnerPlayerState pastOwnerPlayerState, IOwnerPlayerState newOwnerPlayerState)
        {
        }

        public void OnUpdate(IOwnerPlayerState ownerPlayerState)
        {
            if (ownerPlayerState is IHandsOwnerPlayerState)
            {
                return;
            }
        }

        private void TryUpdateNotOwnerPlayerStateBroadcastForServer(
            IOwnerPlayerState newOwnerPlayerState)
        {
            if (_stateTypeForSend.HasValue)
            {
                _deferredActionInvoker.WaitEndFrameAndInvokeAction();
            }
        }

        private bool TryUpdateCurrentStateTypeForSend(IOwnerPlayerState newOwnerPlayerState)
        {
            var currentStateTypeForSend = _stateTypeForSend;

            if (newOwnerPlayerState is ISyncableOwnerPlayerState state)
            {
                var stateType = state.GetSharedStateType();

                if (stateType == currentStateTypeForSend)
                    return false;

                _stateTypeForSend = stateType;
            }
            else if (_sharedStateMachine.CurrentNode.State.GetStateType() != SharedPlayerStateType.Default)
            {
                _stateTypeForSend = SharedPlayerStateType.Default;
            }
            else
            {
                _stateTypeForSend = null;
                return false;
            }

            return true;
        }

        public void OnExitState(IOwnerPlayerState pastPlayerState, IOwnerPlayerState newPlayerState)
        {
            if (newPlayerState is IHandsOwnerPlayerState)
            {
                return;
            }
            
            TryUpdateCurrentStateTypeForSend(newPlayerState);
            TryUpdateNotOwnerPlayerStateBroadcastForServer(newPlayerState);
            
            if (_stateTypeForSend.HasValue)
            {
                _sharedStateMachine.TryExitCurrentState(_sharedStateMachine.Nodes.Values
                    .First(node => node.State.GetStateType() == _stateTypeForSend.Value).State);
            }
        }

        public void OnEnterState(IOwnerPlayerState pastPlayerState, IOwnerPlayerState newPlayerState)
        {
            if (_stateTypeForSend.HasValue)
            {
                _sharedStateMachine.EnterState(_sharedStateMachine.Nodes.Values
                    .First(node => node.State.GetStateType() == _stateTypeForSend.Value).State);
            }
        }
    }
}