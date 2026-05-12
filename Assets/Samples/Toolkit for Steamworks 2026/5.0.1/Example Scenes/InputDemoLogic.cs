#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using Heathen.SteamworksIntegration;
using System.Text;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using UnityEngine;

namespace Heathen.DEMO
{
    public class InputDemoLogic : MonoBehaviour
    {
        [Header("UI References")]
        public TMPro.TextMeshProUGUI label;

        private void Start()
        {
            if (Interface.IsReady)
                ActivateShipControls();
            else
                Interface.OnReady += ActivateShipControls;
        }

        public void UpdateDisplay(InputControllerStateData state)
        {
            StringBuilder sb = new();
            foreach(var inputState in state.inputs)
            {
                sb.Append($"{inputState.name}: {inputState}");
            }
            label.text = sb.ToString();
        }

        public void ActivateMenuControls()
        {
            if (SteamInputManager.Controllers != null && SteamInputManager.Controllers.Count > 0)
                Interface.GetSet("menu_controls").Activate(SteamInputManager.Controllers[0]);
        }

        public void ActivateShipControls()
        {
            if (SteamInputManager.Controllers != null && SteamInputManager.Controllers.Count > 0)
                Interface.GetSet("ship_controls").Activate(SteamInputManager.Controllers[0]);
        }

        public void OpenKnowledgeBaseUserData()
        {
            Application.OpenURL("https://kb.heathen.group/steam/input");
        }
    }
}
#endif