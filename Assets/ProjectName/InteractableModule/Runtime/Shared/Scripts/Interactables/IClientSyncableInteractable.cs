using Cysharp.Threading.Tasks;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public interface IClientSyncableInteractable : IInteractable
    {
        void Accept(INotOwnerInteractionVisitor visitor, IAdditionalInteractionData additionalInteractionData);
        void Accept(IOwnerInteractionVisitor visitor, IFromServerInteractionData interactionData);

        UniTask<bool> CanLocalAcceptAsync(IOwnerInteractionVisitor visitor,
            DataContainer<IAdditionalInteractionData> additionalInteractionData);
    }
}