#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using UnityEngine.Events;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events
{
    [System.Serializable]
    public class LobbyDataEvent : UnityEvent<LobbyData> { }

    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }
}
#endif