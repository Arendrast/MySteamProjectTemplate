using FishNet.Managing.Client;
using FishNet.Transporting;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart;
using Modules.SharedModule.Runtime.Client.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.AppModule.Runtime.Client.Scripts.Infrastructure.Services
{
    public class ClientCoreSynchronizer : IMatchClientService
    {
        private readonly ClientManager _clientManager;
        private readonly ClientsConnectionTrackingService _clientsConnectionTrackingService;
        private readonly EventBus _eventBus;

        public ClientCoreSynchronizer(ClientManager clientManager,
            ClientsConnectionTrackingService clientsConnectionTrackingService, EventBus eventBus, IOwnerSynchronizersMediator ownerSynchronizersMediator)
        {
            _clientManager = clientManager;
            _clientsConnectionTrackingService = clientsConnectionTrackingService;
            _eventBus = eventBus;
            
            ownerSynchronizersMediator.SubscribeToAction(SubscribeClientConnectionTrackingService, UnsubscribeClientConnectionTrackingService, false);
        }

        private void SubscribeClientConnectionTrackingService()
        {
            _clientManager.OnAuthenticated +=
                _clientsConnectionTrackingService.InvokeConnectedActionForOwner;
            _clientManager.OnRemoteConnectionState +=
                InvokeActionByConnectionStateForClientsConnectionTrackingService;
        }

        private void UnsubscribeClientConnectionTrackingService()
        {
            _clientManager.OnAuthenticated -=
                _clientsConnectionTrackingService.InvokeConnectedActionForOwner;
            _clientManager.OnRemoteConnectionState -=
                InvokeActionByConnectionStateForClientsConnectionTrackingService;
        }

        private void InvokeActionByConnectionStateForClientsConnectionTrackingService(RemoteConnectionStateArgs args)
        {
            _clientsConnectionTrackingService.InvokeActionByConnectionState(_clientManager.Clients[args.ConnectionId], args);
        }
    }
}