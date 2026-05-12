#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using Steamworks;
using UnityEngine.Events;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events
{
    [Serializable]
    public class WorkshopItemInstalledEvent : UnityEvent<ItemInstalled_t>
    { }
}
#endif
