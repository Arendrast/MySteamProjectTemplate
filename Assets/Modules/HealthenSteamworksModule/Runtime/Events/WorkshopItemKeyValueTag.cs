#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)

using System;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events
{
    [Serializable]
    public struct WorkshopItemKeyValueTag
    {
        public string key;
        public string value;
    }
}
#endif