using System.Linq;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Object;
using Modules.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts;
using Modules.LevelModule.Runtime.Shared.Scripts;
using Modules.NetworkModule.Runtime.Shared.Scripts.Scene;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.Network.Broadcasts;
using Modules.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;
using Modules.SharedModule.Runtime.Server.Scripts;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.Index;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using Channel = FishNet.Transporting.Channel;

namespace Modules.PlayerModule.Runtime.Server.Scripts
{
    public class ServerPlayersInitializationSynchronizer : IMatchServerService
    {
        private readonly ClientsPlayersFactory _clientsPlayersFactory;
        private readonly ServerManager _serverManager;
        private readonly ClientManager _clientManager;
        private readonly ServerSceneManagementService _serverSceneManagementService;
        private readonly LoadedSceneConnectionsRepository _loadedSceneConnectionsRepository;
        private readonly LevelZoneRepository _levelRepositoryService;

        public ServerPlayersInitializationSynchronizer(
            ServerManager serverManager,
            ClientManager clientManager,
            ServerSceneManagementService serverSceneManagementService,
            LoadedSceneConnectionsRepository loadedSceneConnectionsRepository, LevelZoneFactory levelZoneFactory,
            ClientsPlayersFactory clientsPlayersFactory, IServerSynchronizersMediator mediator,
            LevelZoneRepository levelRepositoryService)
        {
            _serverManager = serverManager;
            _clientManager = clientManager;
            _serverSceneManagementService = serverSceneManagementService;
            _loadedSceneConnectionsRepository = loadedSceneConnectionsRepository;
            _clientsPlayersFactory = clientsPlayersFactory;
            _levelRepositoryService = levelRepositoryService;

            mediator.SubscribeToAction(Subscribe, Unsubscribe);
            mediator.SubscribeToBroadcast<IsOperatorAnswerBroadcast>(HandleIsOperatorQuestionAnswer);
        }

        private void Subscribe()
        {
            _serverSceneManagementService.AddedConnectionToScene += SendIsOperatorQuestionBroadcast;
        }

        private void Unsubscribe()
        {
            _serverSceneManagementService.AddedConnectionToScene -= SendIsOperatorQuestionBroadcast;
        }

        private void SendIsOperatorQuestionBroadcast(NetworkConnection networkConnection)
        {
            _serverManager.Broadcast(networkConnection, new IsOperatorQuestionBroadcast());
        }

        private void HandleIsOperatorQuestionAnswer(NetworkConnection networkConnection,
            IsOperatorAnswerBroadcast broadcast, Channel channel)
        {
            TrySpawnPlayerAndSendInitializeMessageAsync(networkConnection, broadcast.IsOperator);
        }

        private async void TrySpawnPlayerAndSendInitializeMessageAsync(NetworkConnection forConnection, bool isOperator)
        {
            if (!isOperator)
            {
                var clientPlayerComponents = await _clientsPlayersFactory.GetCreatedClientPlayerComponentsAsync(
                    forConnection,
                    await _clientsPlayersFactory.GetCreatedClientPlayerSerializableComponentsAsync(forConnection,
                        _levelRepositoryService.TargetLevelZoneSerializableComponents
                            .SpawnersPositions[_clientManager.Clients.Count - 1].position));

                var enteredPlayerInitializeMessage = new InitializePlayerBroadcast(
                    forConnection.ClientId,
                    clientPlayerComponents.SerializableComponents.GetComponent<NetworkObject>().ObjectId,
                    clientPlayerComponents.ViewComponents.SerializableComponents.GetComponent<NetworkObject>().ObjectId,
                    clientPlayerComponents.ViewComponents.ViewRigSerializableComponents.GetComponent<NetworkObject>()
                        .ObjectId,
                    GetInitializeInventoryItemsMessage(clientPlayerComponents),
                    SharedPlayerStateType.Default, default,
                    clientPlayerComponents.ViewComponents.CharacterType);

                _serverManager.BroadcastToAllWhoLoadedScene(
                    _clientManager,
                    enteredPlayerInitializeMessage,
                    _loadedSceneConnectionsRepository,
                    true,
                    forConnection);
            }

            var initializePlayersMessage = new InitializePlayersBroadcast(
                _clientsPlayersFactory.ClientsComponentsByNetworkConnection.Select(pair =>
                        new InitializePlayerBroadcast(
                            pair.Key.ClientId,
                            pair.Value.SerializableComponents.GetComponent<NetworkObject>().ObjectId,
                            pair.Value.ViewComponents.SerializableComponents.GetComponent<NetworkObject>().ObjectId,
                            pair.Value.ViewComponents.ViewRigSerializableComponents.GetComponent<NetworkObject>()
                                .ObjectId,
                            GetInitializeInventoryItemsMessage(pair.Value),
                            pair.Value.StateMachine.CurrentNode.State.GetStateType(),
                            pair.Value.TargetInteractableDataRepository.TargetData,
                            pair.Value.ViewComponents.CharacterType))
                    .ToArray());

            _serverManager.Broadcast(forConnection, initializePlayersMessage);

            return;

            InitializeInventoryItemsData GetInitializeInventoryItemsMessage(
                ClientPlayerComponents clientPlayerComponents)
            {
                var slotsContainedItemsNumber =
                    clientPlayerComponents.InventoryItemsModel.SlotsContainedTargetItemsNumber;
                return new InitializeInventoryItemsData(clientPlayerComponents.InventoryItemsModel.TargetSlotIndex,
                    clientPlayerComponents.InventoryItemsModel.SlotsTargetItemModels
                        .Select((itemModel, i) =>
                            new ItemSlotData(itemModel?.Config.Id ?? IndexableTools.MissingOrInvalidId,
                                slotsContainedItemsNumber.SafeGet(i)))
                        .ToArray());
            }
        }
    }
}

// tsardnerA