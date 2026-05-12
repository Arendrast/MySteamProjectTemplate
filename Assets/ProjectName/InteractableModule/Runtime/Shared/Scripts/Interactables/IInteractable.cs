using System;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public interface IInteractable
    {
        bool CanInteract { get; set; }
        public event Action Interacted;
    }
}