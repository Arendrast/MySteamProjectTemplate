using UnityEngine;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public abstract class InteractableSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public bool ShowInteractText { get; private set; } = true;
        [field: SerializeField] public bool CanInteract { get; private set; } = true;
    }
}