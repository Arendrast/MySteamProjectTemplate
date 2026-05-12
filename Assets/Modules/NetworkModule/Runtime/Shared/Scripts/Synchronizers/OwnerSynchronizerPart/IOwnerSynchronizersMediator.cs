using System;
using FishNet.Broadcast;
using FishNet.Transporting;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart
{
    public interface IOwnerSynchronizersMediator
    {
        void SubscribeToBroadcast<T>(Action<T, Channel> broadcastAction)
            where T : struct, IBroadcast;

        void SubscribeToAction(Action subscribeAction, Action unsubscribeAction, bool afterInitialize);
    }
}