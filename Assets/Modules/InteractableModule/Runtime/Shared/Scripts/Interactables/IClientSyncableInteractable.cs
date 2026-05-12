using Cysharp.Threading.Tasks;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public interface IClientSyncableInteractable : IInteractable
    {
        void Accept(INotOwnerInteractionVisitor visitor, IAdditionalInteractionData additionalInteractionData);
        void Accept(IOwnerInteractionVisitor visitor, IFromServerInteractionData interactionData);

        UniTask<bool> CanLocalAcceptAsync(IOwnerInteractionVisitor visitor,
            DataContainer<IAdditionalInteractionData> additionalInteractionData);
    }
}