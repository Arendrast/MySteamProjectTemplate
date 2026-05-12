using System;
using System.Collections.Generic;
using FishNet.Broadcast;
using FishNet.Managing.Client;
using FishNet.Transporting;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers;
using MoreLinq;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.BroadcastPart
{
    public class ClientSynchronizersMediator
    {
        private readonly HashSet<IBroadcastSynchronizer> _synchronizersOnInitialize =
            new HashSet<IBroadcastSynchronizer>();

        private readonly HashSet<IBroadcastSynchronizer> _synchronizersAfterInitialize =
            new HashSet<IBroadcastSynchronizer>();
        
        private readonly ActionsSynchronizersMediator
            _actionsSynchronizersMediator = new ActionsSynchronizersMediator();

        private readonly ClientManager _clientManager;

        public ClientSynchronizersMediator(ClientManager clientManager)
        {
            _clientManager = clientManager;
        }

        public void SubscribeToBroadcast<T>(Action<T, Channel> broadcastAction, bool afterInitialize)
            where T : struct, IBroadcast
        {
            var synchronizer = new ClientBroadcastSynchronizer<T>(broadcastAction, _clientManager);

            var syncronizers = afterInitialize
                ? _synchronizersAfterInitialize
                : _synchronizersOnInitialize;

            syncronizers.Add(synchronizer);
        }

        public void SubscribeToAction(Action subscribeAction, Action unsubscribeAction, bool afterInitialize)
        {
            _actionsSynchronizersMediator.SubscribeToAction(subscribeAction, unsubscribeAction, afterInitialize);
        }

        public void Subscribe()
        {
            _synchronizersOnInitialize.ForEach(synchronizer => synchronizer.SubscribeToBroadcast());
            _actionsSynchronizersMediator.Subscribe();
        }

        public void SubscribeAfterInitialize()
        {
            _synchronizersAfterInitialize.ForEach(synchronizer => synchronizer.SubscribeToBroadcast());
            _actionsSynchronizersMediator.SubscribeAfterInitialize();
        }

        public void Unsubscribe()
        {
            _synchronizersOnInitialize.ForEach(synchronizer => synchronizer.UnsubscribeFromBroadcast());
            _synchronizersAfterInitialize.ForEach(synchronizer => synchronizer.UnsubscribeFromBroadcast());
            _actionsSynchronizersMediator.Unsubscribe();
        }
    }
}