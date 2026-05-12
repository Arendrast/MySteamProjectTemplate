using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using ProjectName.CoreModule.Runtime.Server.Scripts.Infrastructure.GameStateMachinePart;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Scene;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using ProjectName.SharedModule.Runtime.Server.Scripts;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure.GameStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using SceneManager = FishNet.Managing.Scened.SceneManager;

namespace ProjectName.CoreModule.Runtime.Server.Scripts.Infrastructure.Services
{
    public class ServerCoreSubscribingMediator : IMatchServerService
    {
        private readonly EventBus _eventBus;
        private readonly ServerGameStateMachine _gameStateMachine;
        private readonly ClientsConnectionTrackingService _clientsConnectionTrackingService;
        private readonly SceneManager _sceneManager;
        private readonly ClientManager _clientManager;
        private readonly ServerSceneManagementService _serverSceneManagementService;
        private readonly ServerManager _serverManager;
        private readonly LoadedSceneConnectionsRepository _loadedSceneConnectionsRepository;

        public ServerCoreSubscribingMediator(
            EventBus eventBus,
            ServerGameStateMachine gameStateMachine, ClientsConnectionTrackingService clientsConnectionTrackingService,
            SceneManager sceneManager, ClientManager clientManager,
            ServerSceneManagementService serverSceneManagementService, ServerManager serverManager,
            LoadedSceneConnectionsRepository loadedSceneConnectionsRepository,
            IServerSynchronizersMediator mediator)
        {
            _eventBus = eventBus;
            _gameStateMachine = gameStateMachine;
            _clientsConnectionTrackingService = clientsConnectionTrackingService;
            _sceneManager = sceneManager;
            _clientManager = clientManager;
            _serverSceneManagementService = serverSceneManagementService;
            _serverManager = serverManager;
            _loadedSceneConnectionsRepository = loadedSceneConnectionsRepository;
            
            mediator.SubscribeToAction(Subscribe, Unsubscribe);
        }

        public void Subscribe()
        {
            _eventBus.Subscribe<EnterServerStateEvent>(EnterServerStateEvent);
            _sceneManager.OnClientPresenceChangeEnd += _serverSceneManagementService.TryInvokingClientLoadedScene;
            _serverManager.OnRemoteConnectionState += _clientsConnectionTrackingService.InvokeActionByConnectionState;
            _serverManager.OnAuthenticationResult += LoadConnectionSceneForClient;
            _serverSceneManagementService.LoadedSceneClient += _loadedSceneConnectionsRepository.Add;
            _clientsConnectionTrackingService.Disconnected += Remove;
        }

        public void Unsubscribe()
        {
            _eventBus.Unsubscribe<EnterServerStateEvent>(EnterServerStateEvent);
            _sceneManager.OnClientPresenceChangeEnd -= _serverSceneManagementService.TryInvokingClientLoadedScene;
            _serverManager.OnRemoteConnectionState -= _clientsConnectionTrackingService.InvokeActionByConnectionState;
            _serverManager.OnAuthenticationResult -= LoadConnectionSceneForClient;
            _serverSceneManagementService.LoadedSceneClient -= _loadedSceneConnectionsRepository.Add;
            _clientsConnectionTrackingService.Disconnected -= Remove;
        }

        private void Remove(NetworkConnection connection)
        {
            _loadedSceneConnectionsRepository.RemoveAll(connection.CustomEquals);
        }
        
        private void LoadConnectionSceneForClient(NetworkConnection networkConnection, bool isAuthPassed)
        {
            if (!isAuthPassed || networkConnection.IsOwnerOrInvalid(_clientManager) || _clientManager.IsOwnerIdInvalid())
                return;

            _serverSceneManagementService.AddConnectionToActiveScene(networkConnection);
        }

        private void EnterServerStateEvent(EnterServerStateEvent @event) =>
            _gameStateMachine.Enter(@event.GameStateType, @event.Connection);
    }
}