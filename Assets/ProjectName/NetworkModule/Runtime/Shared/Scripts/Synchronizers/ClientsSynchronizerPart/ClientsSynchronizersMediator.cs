using System;
using FishNet.Broadcast;
using FishNet.Managing.Client;
using FishNet.Transporting;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.BroadcastPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.SubscribingMediators;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart
{
    public class ClientsSynchronizersMediator : IClientSubscribingMediator,
        IServerSubscribingMediator,
        ISubscribingMediatorAfterInitialize, IClientsSynchronizersMediator, IMatchSharedService
    {
        private readonly ClientSynchronizersMediator _clientSynchronizersMediator;

        public ClientsSynchronizersMediator(ClientManager clientManager)
        {
            _clientSynchronizersMediator = new ClientSynchronizersMediator(clientManager);
        }

        public void SubscribeToBroadcast<T>(Action<T, Channel> broadcastAction)
            where T : struct, IBroadcast
        {
            _clientSynchronizersMediator.SubscribeToBroadcast(broadcastAction, false);
        }

        public void SubscribeToAction(Action subscribeAction, Action unsubscribeAction, bool afterInitialize = false)
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