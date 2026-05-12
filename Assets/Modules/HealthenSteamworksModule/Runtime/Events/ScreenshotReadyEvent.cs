#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using UnityEngine.Events;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events
{
    [System.Serializable]
    public class ScreenshotReadyEvent : UnityEvent<ScreenshotReady> { }
}
#endif