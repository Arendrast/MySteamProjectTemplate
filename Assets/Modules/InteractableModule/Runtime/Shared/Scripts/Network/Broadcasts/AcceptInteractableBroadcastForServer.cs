using FishNet.Broadcast;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public readonly struct AcceptInteractableBroadcastForServer : IBroadcast
    {
        public readonly InteractableData InteractableData;

        public AcceptInteractableBroadcastForServer(InteractableData interactableData)
        {
            InteractableData = interactableData;
        }
    }
}