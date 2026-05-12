using System;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Transporting;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Services
{
    public class ClientsConnectionTrackingService : IMatchSharedService
    {
        public event Action<NetworkConnection> Disconnected, Connected;
        public event Action OwnerDisconnected, OwnerConnected;
        
        private readonly ClientManager _clientManager;

        public ClientsConnectionTrackingService(ClientManager clientManager)
        {
            _clientManager = clientManager;
        }

        public void InvokeConnectedActionForOwner()
        {
            Connected?.Invoke(_clientManager.GetOwnerConnection());
            OwnerConnected?.Invoke();
        }

        public void TryInvokeDisconnectedActionForOwner(ClientConnectionStateArgs args)
        {
            if (args.ConnectionState != LocalConnectionState.Stopped) return;
            
            Disconnected?.Invoke(_clientManager.GetOwnerConnection());
            OwnerDisconnected?.Invoke();
        }

        public void InvokeActionByConnectionState(NetworkConnection connection, RemoteConnectionStateArgs args)
        {
            if (connection.IsOwnerOrInvalid(_clientManager) || _clientManager.IsOwnerIdInvalid())
                return;

            if (args.ConnectionState == RemoteConnectionState.Started)
                Connected?.Invoke(connection);
            else
                Disconnected?.Invoke(connection);
        }
    }
}