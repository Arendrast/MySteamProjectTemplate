#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using Steamworks;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events
{
    [Serializable]
    public struct WorkshopItemPreviewFile
    {
        public string source;
        /// <summary>
        /// YouTubeVideo and Sketchfab are not currently supported
        /// </summary>
        public EItemPreviewType type;
    }
}
#endif