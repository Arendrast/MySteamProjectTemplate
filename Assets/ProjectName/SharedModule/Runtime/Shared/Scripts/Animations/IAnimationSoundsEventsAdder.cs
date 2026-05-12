using System;
using System.Collections.Generic;
using Animancer;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Animations
{
    public class AnimationEventsAdder
    {
        private IEventsByIdProvider _eventsByIdProvider;

        private readonly HashSet<KeyValuePair<int, AnimancerState>> _registeredStatesEventsPairs =
            new HashSet<KeyValuePair<int, AnimancerState>>();

        public void SetEventsProvider(IEventsByIdProvider provider)
        {
            _eventsByIdProvider = provider;
        }

        public bool TryAddEvents(AnimancerState animancerState, int id, Action<IAnimancerStateEventConfig> action, object owner)
        {
            if (animancerState == null)
                return false;

            var events = _eventsByIdProvider.GetEvents(id);

            var pair = KeyValuePair.Create(id, animancerState);

            if (events == null || !_registeredStatesEventsPairs.Add(pair))
                return false;

            foreach (var @event in events.Events)
            {
                animancerState.Events(owner).Add(
                    @event.FrameNumber == 0 ? 0 : animancerState.Length / @event.FrameNumber.FromFramesToSeconds(),
                    () => action?.Invoke(@event));
            }

            return true;
        }
    }
}