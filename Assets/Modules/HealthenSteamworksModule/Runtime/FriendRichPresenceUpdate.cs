#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Steamworks;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [Serializable]
    public struct FriendRichPresenceUpdate
    {
        public FriendRichPresenceUpdate_t data;
        public readonly UserData Friend => data.m_steamIDFriend;
        public readonly AppData App => data.m_nAppID;

        public static implicit operator FriendRichPresenceUpdate(FriendRichPresenceUpdate_t native) => new() { data = native };
        public static implicit operator FriendRichPresenceUpdate_t(FriendRichPresenceUpdate heathen) => heathen.data;
    }
}
#endif