using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using Modules.SharedModule.Runtime.Server.Scripts;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.Network
{
    public class ServerPlayersStatesSynchronizer : IMatchServerService
    {
        public ServerPlayersStatesSynchronizer(IServerSynchronizersMediator serverSynchronizersMediator,
            ServerManager serverManager, ClientManager clientManager,
            LoadedSceneConnectionsRepository loadedSceneConnectionsRepository)
        {
            serverSynchronizersMediator.SubscribeToBroadcast<UpdatePlayerSharedStateBroadcastForServer>(SendBroadcastForClients);

            return;
            
            void SendBroadcastForClients(
                NetworkConnection senderConnection,
                UpdatePlayerSharedStateBroadcastForServer broadcast, Channel channel)
            {
                var resultBroadcast =
                    new UpdatePlayerSharedStateBroadcastForClient(broadcast.StateType, senderConnection.ClientId);
                serverManager.BroadcastToAllWhoLoadedScene(clientManager, resultBroadcast,
                    loadedSceneConnectionsRepository, shouldNotSendToOwner: false, exceptConnection: senderConnection);
            }
        }
    }
}