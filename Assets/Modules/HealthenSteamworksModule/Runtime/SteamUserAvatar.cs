#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using FriendsAPI = Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Friends.Client;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    /// <summary>
    /// Applies the avatar of the indicated user to the attached RawImage
    /// </summary>
    [ModularComponent(typeof(SteamUserData), "Avatars", nameof(image))]
    [AddComponentMenu("")]
    [RequireComponent(typeof(SteamUserData))]
    public class SteamUserAvatar : MonoBehaviour
    {
        public RawImage image;

        private SteamUserData m_Inspector;

        private void Awake()
        {
            m_Inspector = GetComponent<SteamUserData>();
            m_Inspector.onChanged.AddListener(HandlePersonaStateChange);            
        }
         
        private void Start()
        {
            if (m_Inspector.Data.IsValid)
            {
                if (App.Initialized && m_Inspector.Data.IsValid)
                {
                    LoadAvatar(m_Inspector.Data);
                }
                else
                    App.onSteamInitialized.AddListener(HandleSteamInitialized);
            }
        }

        private void HandleSteamInitialized()
        {
            LoadAvatar(m_Inspector.Data);

            App.onSteamInitialized.RemoveListener(HandleSteamInitialized);
        }

        private void HandlePersonaStateChange(PersonaStateChange arg)
        {
            if (FriendsAPI.PersonaChangeHasFlag(arg.Flags, EPersonaChange.k_EPersonaChangeAvatar))
            {
                m_Inspector.Data.LoadAvatar(AvatarLoaded);
            }
        }

        public void LoadAvatar(UserData user) => user.LoadAvatar(AvatarLoaded);

        public void LoadAvatar(CSteamID user) => UserData.Get(user).LoadAvatar((r) =>
        {
            if (image == null)
                return;

            image.texture = r;
        });

        public void LoadAvatar(ulong user) => UserData.Get(user).LoadAvatar(AvatarLoaded);

        private void AvatarLoaded(Texture2D texture)
        {
            if (texture != null && image != null)
                image.texture = texture;
        }
    }
}
#endif