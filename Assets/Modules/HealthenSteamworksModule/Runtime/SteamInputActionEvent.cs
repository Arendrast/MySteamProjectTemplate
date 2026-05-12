#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System.Linq;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events;
using UnityEngine;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [ModularEvents(typeof(SteamInputActionData))]
    [AddComponentMenu("")]
    [RequireComponent(typeof(SteamInputActionData))]
    public class SteamInputActionEvent : MonoBehaviour
    {
        [EventField]
        public ActionUpdateEvent onChanged;

        private SteamInputActionData m_Inspector;

        private void Awake()
        {
            m_Inspector = GetComponent<SteamInputActionData>();
            Input.Client.onInputDataChanged.AddListener(HandleEvent);
        }

        private void OnDestroy()
        {
            Input.Client.onInputDataChanged.RemoveListener(HandleEvent);
        }

        private void HandleEvent(InputControllerStateData controller)
        {
            var actionData = controller.changes.FirstOrDefault(p => p.name == m_Inspector.Action.Name);
            if (actionData.name == m_Inspector.Action.Name)
                onChanged.Invoke(actionData);
        }
    }
}
#endif