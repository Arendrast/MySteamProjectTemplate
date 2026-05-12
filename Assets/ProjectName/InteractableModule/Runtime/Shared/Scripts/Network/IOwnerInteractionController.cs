namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Network
{
    public interface IOwnerInteractionController
    {
        void TryInteractWithTargetInteractable(IFromServerInteractionData interactionData);
        void CancelInteractionWithTargetInteractable();
    }
}