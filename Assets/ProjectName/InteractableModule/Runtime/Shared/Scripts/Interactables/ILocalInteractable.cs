using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public interface ILocalInteractable : IInteractable
    {
        void Accept(IOwnerInteractionVisitor visitor);
    }
}