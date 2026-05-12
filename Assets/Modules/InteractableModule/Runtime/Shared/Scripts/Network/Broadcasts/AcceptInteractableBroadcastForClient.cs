using FishNet.Broadcast;
using FishNet.Connection;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public readonly struct AcceptInteractableBroadcastForClient : IBroadcast
    {
        public readonly InteractableData InteractableData;
        public readonly NetworkConnection FromNetworkConnection;

        public AcceptInteractableBroadcastForClient(InteractableData interactableData,
            NetworkConnection fromNetworkConnection)
        {
            InteractableData = interactableData;
            FromNetworkConnection = fromNetworkConnection;
        }
    }
}