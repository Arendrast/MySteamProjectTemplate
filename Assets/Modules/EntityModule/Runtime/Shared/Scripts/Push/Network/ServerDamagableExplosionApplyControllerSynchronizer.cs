using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using Modules.SharedModule.Runtime.Server.Scripts;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push.Network
{
    public class ServerDamagableExplosionApplyControllerSynchronizer : IMatchServerService
    {
        public ServerDamagableExplosionApplyControllerSynchronizer(
            IServerSynchronizersMediator serverSynchronizersMediator, ServerManager serverManager,
            LoadedSceneConnectionsRepository loadedSceneConnections, ClientManager clientManager)
        {
            serverSynchronizersMediator.SubscribeToBroadcast<CreateAndApplyDamagableExplosionApplyControllerBroadcast>(
                SendBroadcast);
            
            return;

            void SendBroadcast(NetworkConnection senderConnection,
                CreateAndApplyDamagableExplosionApplyControllerBroadcast broadcast,
                Channel channel)
            {
                serverManager.BroadcastToAllWhoLoadedScene(clientManager, broadcast, loadedSceneConnections, false,
                    senderConnection);
            }
        }
    }
}