#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using UnityEngine.Events;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events
{
    [Serializable]
    public class LeaderboardScoresDownloadedEvent : UnityEvent<LeaderboardScores>
    { }
}
#endif
