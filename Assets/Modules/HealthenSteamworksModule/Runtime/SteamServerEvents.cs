#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using UnityEngine;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    public class SteamServerEvents : MonoBehaviour
    {
        /// <summary>
        /// An event raised when by Steamworks debugging on disconnected.
        /// This is only available in server builds.
        /// </summary>
        public App.Server.DisconnectedEvent onDisconnected;
        /// <summary>
        /// An event raised by Steamworks debugging on connected.
        /// This is only available in server builds.
        /// </summary>
        public App.Server.ConnectedEvent onConnected;
        /// <summary>
        /// An event raised by Steamworks debugging on failure.
        /// This is only available in server builds.
        /// </summary>
        public App.Server.FailureEvent onFailure;

        private void Awake()
        {
            App.Server.onDisconnected.AddListener(onDisconnected.Invoke);
            App.Server.onConnected.AddListener(onConnected.Invoke);
            App.Server.onFailure.AddListener(onFailure.Invoke);
        }

        private void OnDestroy()
        {
            App.Server.onDisconnected.RemoveListener(onDisconnected.Invoke);
            App.Server.onConnected.RemoveListener(onConnected.Invoke);
            App.Server.onFailure.RemoveListener(onFailure.Invoke);
        }
    }
}
#endif