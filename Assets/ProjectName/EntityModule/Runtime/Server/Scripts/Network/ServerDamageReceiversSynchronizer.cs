using System.Linq;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage.Network.Broadcasts;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;
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
    public class ServerDamageReceiversSynchronizer : IMatchServerService
    {
        private readonly ServerManager _serverManager;
        private readonly DamageReceiversRepository _damageReceiversesModel;
        private readonly HealthModelsRepository _healthModelses;
        private readonly ClientManager _clientManger;
        private readonly LoadedSceneConnectionsRepository _loadedSceneConnections;
        private readonly ServerSceneManagementService _serverSceneManagementService;

        public ServerDamageReceiversSynchronizer(ServerManager serverManager,
            DamageReceiversRepository damageReceiversesModel,
            HealthModelsRepository healthModelses, ClientManager clientManger,
            LoadedSceneConnectionsRepository loadedSceneConnections,
            ServerSceneManagementService serverSceneManagementService, IServerSynchronizersMediator serverSynchronizersMediator)
        {
            _serverManager = serverManager;
            _damageReceiversesModel = damageReceiversesModel;
            _healthModelses = healthModelses;
            _clientManger = clientManger;
            _loadedSceneConnections = loadedSceneConnections;
            _serverSceneManagementService = serverSceneManagementService;
            
            serverSynchronizersMediator.SubscribeToAction(Subscribe, Unsubscribe, false);
        }

        public void Subscribe()
        {
            _damageReceiversesModel.Added += SendInitializeDamageReceiverseBroadcast;
            _serverSceneManagementService.AddedConnectionToScene += SendInitializeDamageReceiverBroadcastForClient;
        }

        public void Unsubscribe()
        {
            _damageReceiversesModel.Added -= SendInitializeDamageReceiverseBroadcast;
            _serverSceneManagementService.AddedConnectionToScene -= SendInitializeDamageReceiverBroadcastForClient;
        }

        private void SendInitializeDamageReceiverBroadcastForClient(NetworkConnection networkConnection)
        {
            _serverManager.Broadcast(new InitializeDamageReceiversBroadcast(
                _damageReceiversesModel.ValueByKey.Keys
                    .Select(id => GetInitializeDamageReceiverData(id, _healthModelses.ValueByKey[id])).ToArray()));
        }

        private void SendInitializeDamageReceiverseBroadcast(int receiverNetworkObjectId,
            DamageReceiverModel damageReceiverModel)
        {
            _serverManager.BroadcastToAllWhoLoadedScene(_clientManger, new InitializeDamageReceiversBroadcast(
                new[]
                {
                    GetInitializeDamageReceiverData(receiverNetworkObjectId,
                        _healthModelses.ValueByKey[receiverNetworkObjectId])
                }), _loadedSceneConnections);
        }

        private InitializeDamageReceiverData GetInitializeDamageReceiverData(int receiverNetworkObjectId,
            HealthModel healthModel)
        {
            return new InitializeDamageReceiverData(receiverNetworkObjectId, healthModel.HealthPoints,
                healthModel.MaxHealthPoints);
        }
    }
}