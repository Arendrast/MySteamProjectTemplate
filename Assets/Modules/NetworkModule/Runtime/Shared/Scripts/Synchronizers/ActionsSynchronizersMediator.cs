using System;
using System.Collections.Generic;
using MoreLinq;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers
{
    public class ActionsSynchronizersMediator
    {
        private readonly HashSet<Action> _actionsOnInitialize = new HashSet<Action>();
        private readonly HashSet<Action> _actionsAfterInitialize = new HashSet<Action>();
        private readonly HashSet<Action> _actionsOnUnsubscribe = new HashSet<Action>();

        public void SubscribeToAction(Action subscribeAction, Action unsubscribeAction, bool afterInitialize)
        {
            var actions = afterInitialize
                ? _actionsAfterInitialize
                : _actionsOnInitialize;

            actions.Add(subscribeAction);
            _actionsOnUnsubscribe.Add(unsubscribeAction);
        }

        public void Subscribe()
        {
            _actionsOnInitialize.ForEach(TryInvokeAction);
        }

        public void SubscribeAfterInitialize()
        {
            _actionsAfterInitialize.ForEach(TryInvokeAction);
        }

        public void Unsubscribe()
        {
            _actionsOnUnsubscribe.ForEach(TryInvokeAction);
        }

        private static void TryInvokeAction(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogError(exception);
            }
        }
    }
}