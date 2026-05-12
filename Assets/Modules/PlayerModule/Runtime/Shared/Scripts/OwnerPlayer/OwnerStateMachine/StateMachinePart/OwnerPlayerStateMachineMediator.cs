using System.Collections.Generic;
using System.Linq;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.Reactors;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.StateMachinePart
{
    public class OwnerPlayerStateMachineMediator
    {
        private readonly List<IOwnerPlayerStateReactor> _subscribers;
        private readonly List<IEnterableExitableOwnerPlayerStateReactor> _enterableExitableSubscribers;

        public OwnerPlayerStateMachineMediator(IEnumerable<IOwnerPlayerStateReactor> subscribers)
        {
            _subscribers = subscribers.ToList();
            _enterableExitableSubscribers = _subscribers.OfType<IEnterableExitableOwnerPlayerStateReactor>().ToList();
        }

        public void SubscribeOnChangeState(IOwnerPlayerState pastState, IOwnerPlayerState newState)
        {
            _subscribers.ForEach(subscriber => subscriber.OnChangeState(pastState, newState));
        }

        public void SubscribeOnEnterState(IOwnerPlayerState pastState, IOwnerPlayerState newState)
        {
            _enterableExitableSubscribers.ForEach(subscriber => subscriber.OnEnterState(pastState, newState));
        }
        
        public void SubscribeOnExitState(IOwnerPlayerState pastState, IOwnerPlayerState newState)
        {
            _enterableExitableSubscribers.ForEach(subscriber => subscriber.OnExitState(pastState, newState));
        }

        public void SubscribeOnUpdate(IOwnerPlayerState ownerPlayerState)
        {
            _subscribers.ForEach(subscriber => subscriber.OnUpdate(ownerPlayerState));
        }
    }
}