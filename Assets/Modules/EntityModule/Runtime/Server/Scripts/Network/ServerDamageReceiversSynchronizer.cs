using System.Linq;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage.Network.Broadcasts;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.EntityModule.Runtime.Shared.Scripts.Repositories;
using Modules.NetworkModule.Runtime.Shared.Scripts.Scene;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using Modules.SharedModule.Runtime.Server.Scripts;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;

namespace Modules.EntityModule.Runtime.Server.Scripts.Network
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