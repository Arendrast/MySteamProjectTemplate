using System;
using System.Collections.Generic;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Transporting;
using Modules.NetworkModule.Runtime.Shared.Scripts.BroadcastPart;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.SubscribingMediators;
using MoreLinq;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart
{
    public class ServerSynchronizersMediator : IServerSubscribingMediator,
        ISubscribingMediatorAfterInitialize,
        IServerSynchronizersMediator, IMatchServerService
    {
        private readonly HashSet<IBroadcastSynchronizer> _synchronizersOnInitialize =
            new HashSet<IBroadcastSynchronizer>();

        private readonly ActionsSynchronizersMediator
            _actionsSynchronizersMediator = new ActionsSynchronizersMediator();

        private readonly ServerManager _serverManager;

        public ServerSynchronizersMediator(ServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        public void SubscribeToBroadcast<T>(Action<NetworkConnection, T, Channel> broadcastAction)
            where T : struct, IBroadcast
        {
            var synchronizer = new ServerBroadcastSynchronizer<T>(broadcastAction, _serverManager);

            var syncronizers = _synchronizersOnInitialize;

            syncronizers.Add(synchronizer);
        }

        public void SubscribeToAction(Action subscribeAction, Action unsubscribeAction, bool afterInitialize = false)
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
            _actionsSynchronizersMediator.SubscribeAfterInitialize();
        }

        public void Unsubscribe()
        {
            _synchronizersOnInitialize.ForEach(synchronizer => synchronizer.UnsubscribeFromBroadcast());
            _actionsSynchronizersMediator.Unsubscribe();
        }
    }
}