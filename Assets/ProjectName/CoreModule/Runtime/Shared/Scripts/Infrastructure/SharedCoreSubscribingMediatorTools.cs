using FishNet.Managing.Client;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart;
using ProjectName.SharedModule.Runtime.Client.Scripts.GameStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public class SharedCoreSynchronizer : IMatchSharedService
    {
        private readonly ClientsConnectionTrackingService _clientsConnectionTrackingService;
        private readonly EventBus _eventBus;
        private readonly ClientManager _clientManager;

        public SharedCoreSynchronizer(ClientsConnectionTrackingService clientsConnectionTrackingService, EventBus eventBus, ClientManager clientManager,
            IOwnerSynchronizersMediator mediator)
        {
            _clientsConnectionTrackingService = clientsConnectionTrackingService;
            _eventBus = eventBus;
            _clientManager = clientManager;
            
            mediator.SubscribeToAction(Subscribe, Unsubscribe, false);
        }

        private void Subscribe()
        {
            _clientsConnectionTrackingService.OwnerDisconnected += EnterToMainMenuGameState;
            _clientManager.OnClientConnectionState +=
                _clientsConnectionTrackingService.TryInvokeDisconnectedActionForOwner;
        }

        private void Unsubscribe()
        {
            _clientsConnectionTrackingService.OwnerDisconnected -= EnterToMainMenuGameState;
            _clientManager.OnClientConnectionState -=
                _clientsConnectionTrackingService.TryInvokeDisconnectedActionForOwner;
        }
        
        private void EnterToMainMenuGameState()
        {
            _eventBus.Fire(new EnterGameStateEvent(GameStateType.HardReset));
        }
    }
}