using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage.Network.Broadcasts;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using ProjectName.SharedModule.Runtime.Server.Scripts;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Server.Scripts.Network
{
    public class ServerDamageDealersSynchronizer : IMatchServerService
    {
        public ServerDamageDealersSynchronizer(ServerManager serverManager, ClientManager clientManager,
            LoadedSceneConnectionsRepository loadedSceneConnectionsRepository,
            IServerSynchronizersMediator serverSynchronizersesMediator)
        {
            serverSynchronizersesMediator.SubscribeToBroadcast<DealDamageBroadcast>(
                SendDealDamageForClients);

            return;
            
            void SendDealDamageForClients(
                NetworkConnection senderConnection,
                DealDamageBroadcast broadcast, Channel channel)
            {
                serverManager.BroadcastToAllWhoLoadedScene(clientManager, broadcast,
                    loadedSceneConnectionsRepository, false, senderConnection);
            }
        }
    }
}