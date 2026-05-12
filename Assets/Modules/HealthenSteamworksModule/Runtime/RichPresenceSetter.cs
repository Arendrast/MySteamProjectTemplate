#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using UnityEngine;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    /// <summary>
    /// You can test and see what values are set at this URL https://steamcommunity.com/dev/testrichpresence
    /// </summary>
    public class RichPresenceSetter : MonoBehaviour
    {
        public bool setOnEnable = true;
        public bool changeWithAppFocus = false;
        public StringKeyValuePair[] withFocus = new StringKeyValuePair[] { new StringKeyValuePair { key = "steam_display", value = "#Status_AtMainMenu" } };
        public StringKeyValuePair[] withoutFocus;

        private void OnEnable()
        {
            if(App.Initialized)
            {
                if (setOnEnable)
                {
                    if (Application.isFocused)
                        Set(withFocus);
                    else
                        Set(withoutFocus);
                }
            }
            else
            {
                App.onSteamInitialized.AddListener(DelayUpdate);
            }

            Application.focusChanged += Application_focusChanged;
        }

        private void DelayUpdate()
        {
            if (setOnEnable)
            {
                if (Application.isFocused)
                    Set(withFocus);
                else
                    Set(withoutFocus);
            }

            App.onSteamInitialized.RemoveListener(DelayUpdate);
        }

        private void OnDisable()
        {
            Application.focusChanged -= Application_focusChanged;
        }

        private void Application_focusChanged(bool focused)
        {
            if (changeWithAppFocus)
            {
                if (focused)
                    Set(withFocus);
                else
                    Set(withoutFocus);
            }
        }

        public void Set(params StringKeyValuePair[] settings)
        {
            foreach(var kvp in settings)
                Friends.Client.SetRichPresence(kvp.key, kvp.value);
        }

        public void Set(string key, string value) => Friends.Client.SetRichPresence(key, value);

        public void Clear() => Friends.Client.ClearRichPresence();
    }
}
#endif