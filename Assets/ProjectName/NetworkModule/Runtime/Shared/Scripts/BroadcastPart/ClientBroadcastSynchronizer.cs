using System;
using FishNet.Broadcast;
using FishNet.Managing.Client;
using FishNet.Transporting;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.BroadcastPart
{
    public class ClientBroadcastSynchronizer<T> : IBroadcastSynchronizer where T : struct, IBroadcast
    {
        private readonly ClientManager _clientManager;
        private readonly Action<T, Channel> _broadcast;

        public ClientBroadcastSynchronizer(Action<T, Channel> broadcast, ClientManager clientManager)
        {
            _broadcast = broadcast;
            _clientManager = clientManager;
        }

        public void SubscribeToBroadcast()
        {
            _clientManager.RegisterBroadcast(_broadcast);
        }

        public void UnsubscribeFromBroadcast()
        {
            _clientManager.UnregisterBroadcast(_broadcast);
        }
    }
}