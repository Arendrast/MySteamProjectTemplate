#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Steamworks;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events
{
    public struct WorkshopItemDataCreateStatus
    {
        public bool hasError;
        public string errorMessage;
        public WorkshopItemEditorData data;
        public CreateItemResult_t? createItemResult;
        public SubmitItemUpdateResult_t? submitItemUpdateResult;
    }
}
#endif