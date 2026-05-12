using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Object;
using Modules.InteractableModule.Runtime.Shared.Scripts.Interactables;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts;
using Modules.NetworkModule.Runtime.Shared.Scripts.Scene;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using Modules.SharedModule.Runtime.Server.Scripts;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.Serialization;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using Newtonsoft.Json;
using Channel = FishNet.Transporting.Channel;

namespace Modules.InteractableModule.Runtime.Server.Scripts.Network
{
    public class ServerInteractablesSynchronizer : IMatchServerService
    {
        private readonly ServerManager _serverManager;
        private readonly ClientManager _clientManager;
        private readonly LoadedSceneConnectionsRepository _loadedSceneConnectionsRepository;
        private readonly InteractablesRepository _interactablesesRepository;
        private readonly ServerSceneManagementService _serverSceneManagementService;
        private readonly IClientsTargetInteractableDataRepositoriesProvider _repositoriesProvider;

        public ServerInteractablesSynchronizer(ServerManager serverManager, ClientManager clientManager,
            LoadedSceneConnectionsRepository loadedSceneConnectionsRepository,
            InteractablesRepository interactablesesRepository,
            ServerSceneManagementService serverSceneManagementService,
            IClientsTargetInteractableDataRepositoriesProvider repositoriesProvider,
            IServerSynchronizersMediator mediator)
        {
            _serverManager = serverManager;
            _clientManager = clientManager;
            _loadedSceneConnectionsRepository = loadedSceneConnectionsRepository;
            _interactablesesRepository = interactablesesRepository;
            _serverSceneManagementService = serverSceneManagementService;
            _repositoriesProvider = repositoriesProvider;

            mediator.SubscribeToBroadcast<AcceptInteractableBroadcastForServer>(HandleAcceptInteractableBroadcastAsync);
            mediator.SubscribeToBroadcast<CancelInteractionBroadcastForServer>(HandleCancelInteractionBroadcast);
            mediator.SubscribeToAction(Subscribe, Unsubscribe, false);
        }

        private void Subscribe()
        {
            _serverSceneManagementService.AddedConnectionToScene += SendInitializeInteractablesBroadcastAsync;
            _interactablesesRepository.Added += TrySendInitializeInteractableseBroadcastAsync;
        }

        private void Unsubscribe()
        {
            _serverSceneManagementService.AddedConnectionToScene -= SendInitializeInteractablesBroadcastAsync;
            _interactablesesRepository.Added -= TrySendInitializeInteractableseBroadcastAsync;
        }

        private void HandleCancelInteractionBroadcast(NetworkConnection networkConnection,
            CancelInteractionBroadcastForServer broadcastForServer, Channel channel)
        {
            var interactableSerializableComponents = _clientManager
                .TryGetNetworkObjectById(broadcastForServer.NetworkObjectId)?
                .GetComponent<InteractableSerializableComponents>();

            var interactable =
                _interactablesesRepository.ValueByKey.GetValueOrDefault(interactableSerializableComponents);

            if (interactable is not ICancellableServerSyncableInteractable syncableInteractable)
            {
                return;
            }

            syncableInteractable.CancelInteraction(networkConnection);
        }

        private async void HandleAcceptInteractableBroadcastAsync(NetworkConnection senderConnection,
            AcceptInteractableBroadcastForServer broadcastForServer, Channel channel)
        {
            var interactableSerializableComponents = _clientManager
                .TryGetNetworkObjectById(broadcastForServer.InteractableData.NetworkObjectId)?
                .GetComponent<InteractableSerializableComponents>();

            var interactable =
                _interactablesesRepository.ValueByKey.GetValueOrDefault(interactableSerializableComponents);

            var successfullyAccepted = interactableSerializableComponents != null && interactable != null;

            if (!successfullyAccepted || interactable is not IServerSyncableInteractable syncableInteractable)
            {
                _serverManager.Broadcast(senderConnection,
                    new TakeAcceptInteractableResultBroadcastForClient(false, null));
                return;
            }

            var additionalInteractionData = await broadcastForServer.InteractableData.SerializedConcreteInteractableData
                .GetFromJsonDeserializedWithoutNullsAsync() as IAdditionalInteractionData;

            var successfully = syncableInteractable.CanAccept(additionalInteractionData);

            if (!successfully)
            {
                _serverManager.Broadcast(senderConnection,
                    new TakeAcceptInteractableResultBroadcastForClient(false, null));
                return;
            }

            _serverManager.Broadcast(senderConnection,
                new TakeAcceptInteractableResultBroadcastForClient(true,
                    await syncableInteractable.AcceptAndGetFromServerInteractionData(senderConnection,
                        additionalInteractionData).GetJsonSerializedObjectWithoutNullsAsync(TypeNameHandling.All)));

            _repositoriesProvider.Repositories[senderConnection].TargetData = broadcastForServer.InteractableData;

            _serverManager.BroadcastToAllWhoLoadedScene(_clientManager,
                new AcceptInteractableBroadcastForClient(broadcastForServer.InteractableData, senderConnection),
                _loadedSceneConnectionsRepository, false, senderConnection);
        }

        private async void TrySendInitializeInteractableseBroadcastAsync(
            InteractableSerializableComponents interactableSerializableComponents, IInteractable interactable)
        {
            if (interactable is not IServerSyncableInteractable syncableInteractable)
            {
                return;
            }

            var interactablesCreateData = new[]
                    { await GetInteractableDataAsync(interactableSerializableComponents, syncableInteractable) }
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _serverManager.BroadcastToAllWhoLoadedScene(_clientManager,
                new InitializeInteractablesBroadcast(interactablesCreateData.Keys.ToArray(),
                    interactablesCreateData.Values.ToArray()), _loadedSceneConnectionsRepository);
        }

        private async UniTask<KeyValuePair<InteractableData, bool>> GetInteractableDataAsync(
            InteractableSerializableComponents interactableSerializableComponents,
            IServerSyncableInteractable interactable)
        {
            return KeyValuePair.Create(new InteractableData(
                    interactableSerializableComponents.GetComponent<NetworkObject>().ObjectId,
                    await interactable.GetInitializationData()
                        .GetJsonSerializedObjectWithoutNullsAsync(TypeNameHandling.Objects)),
                interactable.CanInteract);
        }

        private async void SendInitializeInteractablesBroadcastAsync(NetworkConnection senderConnection)
        {
            var interactablesData =
                (await UniTask.WhenAll(_interactablesesRepository.ValueByKey
                    .Where(interactable => interactable.Value is IServerSyncableInteractable)
                    .Select(async interactablePair =>
                        await GetInteractableDataAsync(interactablePair.Key,
                            interactablePair.Value as IServerSyncableInteractable))))
                .ToDictionary(key => key.Key, pair => pair.Value);

            _serverManager.Broadcast(senderConnection,
                new InitializeInteractablesBroadcast(interactablesData.Keys.ToArray(),
                    interactablesData.Values.ToArray()));
        }
    }
}