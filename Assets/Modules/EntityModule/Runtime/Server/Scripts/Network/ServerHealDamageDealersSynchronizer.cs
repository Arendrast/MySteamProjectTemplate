using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using Modules.EntityModule.Runtime.Shared.Scripts.Heal.Network.Broadcasts;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using Modules.SharedModule.Runtime.Server.Scripts;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;

namespace Modules.EntityModule.Runtime.Server.Scripts.Network
{
    public class ServerHealDamageDealersSynchronizer : IMatchServerService
    {
        public ServerHealDamageDealersSynchronizer(ServerManager serverManager, ClientManager clientManager,
            LoadedSceneConnectionsRepository loadedSceneConnectionsRepository,
            IServerSynchronizersMediator serverSynchronizersesMediator)
        {
            serverSynchronizersesMediator.SubscribeToBroadcast<DealHealBroadcast>(
                SendDealHealForClients);

            return;
            
            void SendDealHealForClients(
                NetworkConnection senderConnection,
                DealHealBroadcast broadcast, Channel channel)
            {
                serverManager.BroadcastToAllWhoLoadedScene(clientManager, broadcast,
                    loadedSceneConnectionsRepository, false, senderConnection);
            }
        }
    }
}