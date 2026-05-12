using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using ProjectName.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using ProjectName.SharedModule.Runtime.Server.Scripts;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.InventoryModule.Runtime.Server.Scripts.Network
{
    public class ServerInventoryItemsChangeSynchronizer : IMatchServerService
    {
        private readonly ServerManager _serverManager;
        private readonly ClientManager _clientManager;
        private readonly LoadedSceneConnectionsRepository _loadedSceneConnections;

        public ServerInventoryItemsChangeSynchronizer(ServerManager serverManager, ClientManager clientManager, LoadedSceneConnectionsRepository loadedSceneConnections,
            IServerSynchronizersMediator mediator)
        {
            _serverManager = serverManager;
            _clientManager = clientManager;
            _loadedSceneConnections = loadedSceneConnections;
            
            mediator.SubscribeToBroadcast<ChangeTargetSlotBroadcastForServer>(SendBroadcastToClients);
            mediator.SubscribeToBroadcast<AddSlotItemBroadcastForServer>(SendBroadcastToClients);
            mediator.SubscribeToBroadcast<RemoveSlotItemBroadcastForServer>(SendBroadcastToClients);
        }
        
        private void SendBroadcastToClients(NetworkConnection senderConnection, 
            RemoveSlotItemBroadcastForServer broadcast, Channel channel)
        {
            var resultBroadcast =
                new RemoveSlotItemBroadcastForClient(senderConnection.ClientId, broadcast.SlotIndex, broadcast.OnlyOne);
            _serverManager.BroadcastToAllWhoLoadedScene(_clientManager, resultBroadcast, 
                _loadedSceneConnections, false, senderConnection);
        }
        
        private void SendBroadcastToClients(NetworkConnection senderConnection, 
            AddSlotItemBroadcastForServer broadcast, Channel channel)
        {
            var resultBroadcast =
                new AddSlotItemBroadcastForClient(senderConnection.ClientId, broadcast.ItemId, broadcast.SlotIndex);
            _serverManager.BroadcastToAllWhoLoadedScene(_clientManager, resultBroadcast, 
                _loadedSceneConnections, false, senderConnection);
        }

        private void SendBroadcastToClients(NetworkConnection senderConnection,
            ChangeTargetSlotBroadcastForServer broadcast, Channel channel)
        {
            var resultBroadcast =
                new ChangeTargetSlotBroadcastForClient(senderConnection.ClientId, broadcast.SlotIndex);
            _serverManager.BroadcastToAllWhoLoadedScene(_clientManager, resultBroadcast, 
                _loadedSceneConnections, false, senderConnection);
        }
    }
}