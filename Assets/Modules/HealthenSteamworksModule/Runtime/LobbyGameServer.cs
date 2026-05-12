#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using Steamworks;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [Serializable]
    public struct LobbyGameServer
    {
        public CSteamID id;
        public string IpAddress
        {
            get => Utilities.IPUintToString(ipAddress);
            set => ipAddress = Utilities.IPStringToUint(value);
        }
        public uint ipAddress;
        public ushort port;
    }
    //*/

}
#endif