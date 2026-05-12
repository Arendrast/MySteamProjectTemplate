using FishNet.Connection;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public interface ICancellableServerSyncableInteractable : IServerSyncableInteractable
    {
        void CancelInteraction(NetworkConnection networkConnection);
    }
}