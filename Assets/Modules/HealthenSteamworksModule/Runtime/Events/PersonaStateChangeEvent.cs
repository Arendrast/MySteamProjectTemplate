#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using Steamworks;
using UnityEngine.Events;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events
{
    /// <summary>
    /// A custom serializable <see cref="UnityEvent{T0}"/> which handles <see cref="PersonaStateChange_t"/> data.
    /// </summary>
    [Serializable]
    public class PersonaStateChangeEvent : UnityEvent<PersonaStateChange>
    { }
}
#endif
