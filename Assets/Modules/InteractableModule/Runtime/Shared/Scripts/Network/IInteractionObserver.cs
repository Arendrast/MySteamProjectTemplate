using System;
using Modules.InteractableModule.Runtime.Shared.Scripts.Interactables;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Network
{
    public interface IInteractionObserver
    {
        event Action<IInteractable, IAdditionalInteractionData> StartedInteraction;
        event Action<IInteractable> CancelledApprovedInteraction;
    }
}