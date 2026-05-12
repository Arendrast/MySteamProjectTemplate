#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using Heathen.SteamworksIntegration;
using System;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime;
using UnityEngine;
using UnityEngine.Events;
using CloudSave = Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.RemoteStorage.Client;

namespace Heathen.DEMO
{
    public class DemoDataDisplayRecord : MonoBehaviour
    {
        public UnityEvent<ulong, byte[]> AuthenticationProcessor;

        [SerializeField]
        private TMPro.TextMeshProUGUI title;

        private RemoteStorageFile record;
        private Action<RemoteStorageFile> callback;

        public void Initialize(RemoteStorageFile file, Action<RemoteStorageFile> loadCallback)
        {
            record = file;
            title.text = record.name;
            callback = loadCallback;
        }

        public void Delete() => CloudSave.FileDelete(record.name);

        public void Load() => callback?.Invoke(record);
    }
}
#endif