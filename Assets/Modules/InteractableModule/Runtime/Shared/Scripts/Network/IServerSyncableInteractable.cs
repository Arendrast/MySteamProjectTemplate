using FishNet.Connection;
using Modules.InteractableModule.Runtime.Shared.Scripts.Interactables;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Network
{
    public interface IServerSyncableInteractable : IInteractable
    {
        bool CanAccept(IAdditionalInteractionData data);

        IFromServerInteractionData AcceptAndGetFromServerInteractionData(NetworkConnection networkConnection, 
            IAdditionalInteractionData data);

        IInteractableInitializationData GetInitializationData();
    }
}