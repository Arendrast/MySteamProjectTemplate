#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Steamworks;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [Serializable]
    public struct ClanChatMsg
    {
        /// <summary>
        /// The room this message relates to
        /// </summary>
        /// <remarks>
        /// The room.id will always be populated however under some conditions it is possible to receive a clan chat room message from a room the internal system is not aware of.
        /// In that event the clan.id will be invalid and the room.enterResponse will be Failed
        /// </remarks>
        public ChatRoom room;
        public EChatEntryType type;
        public string message;
        public UserData user;
    }
}
#endif