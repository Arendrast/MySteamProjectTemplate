using System;
using FishNet.Broadcast;
using FishNet.Managing.Client;
using FishNet.Transporting;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.BroadcastPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.SubscribingMediators;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart
{
    public class OwnerSynchronizersMediator : IClientSubscribingMediator, IServerSubscribingMediator, ISubscribingMediatorAfterInitialize,
        IOwnerSynchronizersMediator, IMatchSharedService
    {
        private readonly ClientSynchronizersMediator _clientSynchronizersMediator;

        public OwnerSynchronizersMediator(ClientManager clientManager)
        {
            _clientSynchronizersMediator = new ClientSynchronizersMediator(clientManager);
        }

        public void SubscribeToBroadcast<T>(Action<T, Channel> broadcastAction)
            where T : struct, IBroadcast
        {
            _clientSynchronizersMediator.SubscribeToBroadcast(broadcastAction, false);
        }

        public void SubscribeToAction(Action subscribeAction, Action unsubscribeAction, bool afterInitialize)
        {
            _clientSynchronizersMediator.SubscribeToAction(subscribeAction, unsubscribeAction, afterInitialize);
        }
        
        public void Subscribe()
        {
            _clientSynchronizersMediator.Subscribe();
        }

        public void SubscribeAfterInitialize()
        {
            _clientSynchronizersMediator.SubscribeAfterInitialize();
        }

        public void Unsubscribe()
        {
            _clientSynchronizersMediator.Unsubscribe();
        }
    }
}