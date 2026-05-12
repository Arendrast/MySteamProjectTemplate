using FishNet.Connection;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public interface ICancellableServerSyncableInteractable : IServerSyncableInteractable
    {
        void CancelInteraction(NetworkConnection networkConnection);
    }
}