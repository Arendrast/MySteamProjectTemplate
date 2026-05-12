#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{

    [Serializable]
    public struct StringKeyValuePair
    {
        public string key;
        public string value;
    }
}
#endif