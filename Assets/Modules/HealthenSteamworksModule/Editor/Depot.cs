#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using UnityEditor;

namespace Modules.HealthenSteamworksModule.Editor
{
    [System.Serializable]
    public class Depot
    {
        public string name;
        public uint id;
        public BuildTarget target;
        public string extension;
    }
}
#endif