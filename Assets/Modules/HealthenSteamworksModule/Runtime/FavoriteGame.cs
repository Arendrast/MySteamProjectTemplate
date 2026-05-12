#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using Steamworks;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [Serializable]
    public struct FavoriteGame
    {
        public AppId_t appId;
        public string IpAddress
        {
            get => Utilities.IPUintToString(ipAddress);
            set => ipAddress = Utilities.IPStringToUint(value);
        }
        public uint ipAddress;
        public ushort connectionPort;
        public ushort queryPort;
        public DateTime lastPlayedOnServer;
        public bool isHistory;
    }
}
#endif