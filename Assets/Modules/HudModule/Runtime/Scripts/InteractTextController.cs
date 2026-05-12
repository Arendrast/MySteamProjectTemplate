using Modules.InteractableModule.Runtime.Shared.Scripts.Interactables;
using Modules.PlayerModule.Runtime.Shared.Scripts.Interaction;
using TMPro;

namespace Modules.HudModule.Runtime.Scripts
{
    public class InteractTextController
    {
        public InteractTextController(TextMeshProUGUI interactText, PlayerInteractionController controller)
        {
            interactText.gameObject.SetActive(false);
            controller.DetectedInteractable += SetActiveInteractText;

            controller.NotDetectedInteractable += () => interactText.gameObject.SetActive(false);
            controller.StartedInteraction += (_, _) => interactText.gameObject.SetActive(false);

            return;

            void SetActiveInteractText(IInteractable interactable,
                InteractableSerializableComponents interactableSerializableComponents)
            {
                interactText.gameObject.SetActive(interactable != null && interactableSerializableComponents.ShowInteractText && interactable is
                {
                    CanInteract: true
                });
            }
        }
    }
}