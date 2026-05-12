using Modules.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public interface ILocalInteractable : IInteractable
    {
        void Accept(IOwnerInteractionVisitor visitor);
    }
}