#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data
{
    public enum SteamLobbyModeType
    {
        General,
        Session,
        Party
    }
}
#endif