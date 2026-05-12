using System;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart
{
    public interface IServerSynchronizersMediator
    {
        void SubscribeToBroadcast<T>(Action<NetworkConnection, T, Channel> broadcastAction)
            where T : struct, IBroadcast;

        void SubscribeToAction(Action subscribeAction, Action unsubscribeAction, bool afterInitialize = false);
    }
}