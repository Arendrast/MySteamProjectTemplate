using FishNet.Object;
using UnityEngine;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    [RequireComponent(typeof(NetworkObject))]
    public abstract class InteractableSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public bool ShowInteractText { get; private set; } = true;
        [field: SerializeField] public bool CanInteract { get; private set; } = true;
    }
}