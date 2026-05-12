using System;
using FishNet.Broadcast;
using FishNet.Transporting;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart
{
    public interface IClientsSynchronizersMediator
    {
        void SubscribeToBroadcast<T>(Action<T, Channel> broadcastAction)
            where T : struct, IBroadcast;

        void SubscribeToAction(Action subscribeAction, Action unsubscribeAction, bool afterInitialize = false);
    }
}