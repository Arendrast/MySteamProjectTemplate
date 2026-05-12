#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using System.Collections.Generic;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [Serializable]
    public struct LeaderboardScores
    {
        public bool bIOFailure;
        public bool playerIncluded;
        public List<LeaderboardEntry> scoreData;
    }
}
#endif
