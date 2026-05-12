using System.Linq;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Heal;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Heal.Network.Broadcasts;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Repositories;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Scene;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using ProjectName.SharedModule.Runtime.Server.Scripts;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Server.Scripts.Network
{
    public class ServerHealReceiversSynchronizer : IMatchServerService
    {
        private readonly ServerManager _serverManager;
        private readonly HealReceiversRepository _healReceiversesModel;
        private readonly HealthModelsRepository _healthModelses;
        private readonly ClientManager _clientManger;
        private readonly LoadedSceneConnectionsRepository _loadedSceneConnections;
        private readonly ServerSceneManagementService _serverSceneManagementService;

        public ServerHealReceiversSynchronizer(ServerManager serverManager,
            HealReceiversRepository healReceiversesModel,
            HealthModelsRepository healthModelses, ClientManager clientManger,
            LoadedSceneConnectionsRepository loadedSceneConnections,
            ServerSceneManagementService serverSceneManagementService,
            IServerSynchronizersMediator serverSynchronizersMediator)
        {
            _serverManager = serverManager;
            _healReceiversesModel = healReceiversesModel;
            _healthModelses = healthModelses;
            _clientManger = clientManger;
            _loadedSceneConnections = loadedSceneConnections;
            _serverSceneManagementService = serverSceneManagementService;

            serverSynchronizersMediator.SubscribeToAction(Subscribe, Unsubscribe, false);
        }

        public void Subscribe()
        {
            _healReceiversesModel.Added += SendInitializeHealReceiverseBroadcast;
            _serverSceneManagementService.AddedConnectionToScene += SendInitializeHealReceiverBroadcastForClient;
        }

        public void Unsubscribe()
        {
            _healReceiversesModel.Added -= SendInitializeHealReceiverseBroadcast;
            _serverSceneManagementService.AddedConnectionToScene -= SendInitializeHealReceiverBroadcastForClient;
        }

        private void SendInitializeHealReceiverBroadcastForClient(NetworkConnection networkConnection)
        {
            _serverManager.Broadcast(new InitializeHealReceiversBroadcast(
                _healReceiversesModel.ValueByKey.Keys
                    .Select(id => GetInitializeHealReceiverData(id, _healthModelses.ValueByKey[id])).ToArray()));
        }

        private void SendInitializeHealReceiverseBroadcast(int receiverNetworkObjectId,
            HealReceiverModel healReceiverModel)
        {
            _serverManager.BroadcastToAllWhoLoadedScene(_clientManger, new InitializeHealReceiversBroadcast(
                new[]
                {
                    GetInitializeHealReceiverData(receiverNetworkObjectId,
                        _healthModelses.ValueByKey[receiverNetworkObjectId])
                }), _loadedSceneConnections);
        }

        private InitializeHealReceiverData GetInitializeHealReceiverData(int receiverNetworkObjectId,
            HealthModel healthModel)
        {
            return new InitializeHealReceiverData(receiverNetworkObjectId, healthModel.HealthPoints,
                healthModel.MaxHealthPoints);
        }
    }
}