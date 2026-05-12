#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Steamworks;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [Serializable]
    public struct PartyBeaconDetails
    {
        public PartyBeaconID_t id;
        public UserData owner;
        public SteamPartyBeaconLocation_t location;
        public string metadata;
    }
    //*/

}
#endif