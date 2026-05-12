using System;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Network
{
    public interface IInteractionObserver
    {
        event Action<IInteractable, IAdditionalInteractionData> StartedInteraction;
        event Action<IInteractable> CancelledApprovedInteraction;
    }
}