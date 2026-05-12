#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System.Linq;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    public struct InputActionSetLayerData
    {
        public string layerName;

        public InputActionSetData Data { get; private set; }

        public bool IsActive(Steamworks.InputHandle_t controller)
        {
            if (Data == 0)
                Data = InputActionSetData.Get(layerName);

            if (Data != 0)
            {
                var layers = Input.Client.GetActiveActionSetLayers(controller);
                ulong handle = Data;
                if (layers.Any(p => p.m_InputActionSetHandle == handle))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public void Activate(Steamworks.InputHandle_t controller)
        {
            if (Data == 0)
                Data = InputActionSetData.Get(layerName);

            if (Data != 0)
            {
                Input.Client.ActivateActionSetLayer(controller, Data);
            }
        }

        public void Activate()
        {
            if (Data == 0)
                Data = InputActionSetData.Get(layerName);

            if (Data != 0)
            {
                Input.Client.ActivateActionSetLayer(Data);
            }
        }
    }
}
#endif