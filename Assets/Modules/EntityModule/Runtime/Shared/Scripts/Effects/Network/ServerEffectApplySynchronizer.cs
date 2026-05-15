using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using Modules.SharedModule.Runtime.Server.Scripts;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Network
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
                Debug.Log(2);
                serverManager.BroadcastToAllWhoLoadedScene(clientManager, broadcast, loadedSceneConnections, false,
                    senderConnection);
            }
        }
    }
}