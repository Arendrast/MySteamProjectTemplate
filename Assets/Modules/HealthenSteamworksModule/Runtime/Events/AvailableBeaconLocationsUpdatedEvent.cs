#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using Steamworks;
using UnityEngine.Events;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events
{
    [System.Serializable]
    public class AvailableBeaconLocationsUpdatedEvent : UnityEvent<AvailableBeaconLocationsUpdated_t> { }
}
#endif