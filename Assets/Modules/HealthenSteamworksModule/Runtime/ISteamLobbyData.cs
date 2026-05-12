using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;

#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    public interface ISteamLobbyData
    {
        public LobbyData Data { get; set; }
    }
}
#endif