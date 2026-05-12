namespace Modules.InteractableModule.Runtime.Shared.Scripts.Network
{
    public interface IOwnerInteractionObserverProvider
    {
        IInteractionObserver InteractionObserver { get; }
    }
}