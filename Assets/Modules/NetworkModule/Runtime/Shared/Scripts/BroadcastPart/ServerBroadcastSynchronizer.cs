using System;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Transporting;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.BroadcastPart
{
    public class ServerBroadcastSynchronizer<T> : IBroadcastSynchronizer where T : struct, IBroadcast
    {
        private readonly ServerManager _serverManager;
        private readonly Action<NetworkConnection, T, Channel> _broadcast;

        public ServerBroadcastSynchronizer(Action<NetworkConnection, T, Channel> broadcast, ServerManager serverManager)
        {
            _broadcast = broadcast;
            _serverManager = serverManager;
        }

        public void SubscribeToBroadcast()
        {
            _serverManager.RegisterBroadcast(_broadcast);
        }

        public void UnsubscribeFromBroadcast()
        {
            _serverManager.UnregisterBroadcast(_broadcast);
        }
    }
}