using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using ProjectName.SharedModule.Runtime.Server.Scripts;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Network
{
    public class ServerEffectApplySynchronizer : IMatchServerService
    {
        public ServerEffectApplySynchronizer(
            IServerSynchronizersMediator serverSynchronizersMediator, ServerManager serverManager,
            LoadedSceneConnectionsRepository loadedSceneConnections, ClientManager clientManager)
        {
            serverSynchronizersMediator.SubscribeToBroadcast<ApplyOrCancelEffectBroadcast>(
                SendBroadcast);
            
            return;

            void SendBroadcast(NetworkConnection senderConnection,
                ApplyOrCancelEffectBroadcast broadcast,
                Channel channel)
            {
                serverManager.BroadcastToAllWhoLoadedScene(clientManager, broadcast, loadedSceneConnections, false,
                    senderConnection);
            }
        }
    }
}