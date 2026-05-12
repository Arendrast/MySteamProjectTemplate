#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using Steamworks;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [Serializable]
    public struct InventoryResult
    {
        public ItemDetail[] items;
        public EResult result;
        public DateTime timestamp;
    }
}
#endif