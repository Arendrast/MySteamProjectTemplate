using System.Collections.Generic;
using System.Linq;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.SharedModule.Runtime.Server.Scripts
{
    public static class HostNetworkTools
    {
        private static readonly HashSet<NetworkConnection> _exceptConnections = new HashSet<NetworkConnection>();

        public static void BroadcastToAllWhoLoadedScene<TBroadcast>(this ServerManager serverManager,
            ClientManager clientManager,
            TBroadcast broadcast, LoadedSceneConnectionsRepository loadedSceneConnections,
            bool shouldNotSendToOwner = true, NetworkConnection exceptConnection = null)
            where TBroadcast : struct, IBroadcast
        {
            _exceptConnections.Clear();

            if (exceptConnection != null)
                _exceptConnections.Add(exceptConnection);

            var ownerConnection = clientManager.GetOwnerConnection();

            if (!shouldNotSendToOwner && !_exceptConnections.Contains(ownerConnection))
                serverManager.Broadcast(ownerConnection, broadcast);

            foreach (var connection in loadedSceneConnections.Except(_exceptConnections))
            {
                serverManager.Broadcast(connection, broadcast);
            }
        }
    }
}