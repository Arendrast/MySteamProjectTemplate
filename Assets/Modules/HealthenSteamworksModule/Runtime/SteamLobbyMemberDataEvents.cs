#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [ModularEvents(typeof(SteamLobbyMemberData))]
    [AddComponentMenu("")]
    [RequireComponent(typeof(SteamLobbyMemberData))]
    public class SteamLobbyMemberDataEvents : MonoBehaviour
    {
        [EventField]
        public UnityEvent<bool> onIsLobbyOwnerStatus;
        [EventField]
        public UnityEvent<bool> onReadyChanged;
        [EventField]
        public UnityEvent<LobbyDataUpdateEventData> onMetadataChanged;

        private SteamLobbyMemberData m_SteamLobbyMemberData;
        private bool m_Ready = false;

        private void Awake()
        {
            m_SteamLobbyMemberData = GetComponent<SteamLobbyMemberData>();
            Matchmaking.Client.OnLobbyDataUpdate.AddListener(GlobalDataUpdate);
            Matchmaking.Client.OnLobbyChatUpdate.AddListener(ChatStateUpdate);
            m_Ready = m_SteamLobbyMemberData.Data.IsReady;
            onReadyChanged?.Invoke(m_Ready);
        }

        private void OnDestroy()
        {
            Matchmaking.Client.OnLobbyDataUpdate.RemoveListener(GlobalDataUpdate);
            Matchmaking.Client.OnLobbyChatUpdate.RemoveListener(ChatStateUpdate);
        }
        
        private void GlobalDataUpdate(LobbyDataUpdateEventData arg0)
        {
            if (arg0.lobby == m_SteamLobbyMemberData.Data.lobby && arg0.member.HasValue && arg0.member.Value == m_SteamLobbyMemberData.Data)
            {
                onIsLobbyOwnerStatus?.Invoke(arg0.lobby.Owner.user == m_SteamLobbyMemberData.Data.user);

                if (m_Ready != m_SteamLobbyMemberData.Data.IsReady)
                {
                    onReadyChanged?.Invoke(m_SteamLobbyMemberData.Data.IsReady);
                }

                onMetadataChanged?.Invoke(arg0);
            }
        }

        private void ChatStateUpdate(LobbyChatUpdate_t arg0)
        {
            LobbyData updatedLobby = arg0.m_ulSteamIDLobby;
            if (updatedLobby == m_SteamLobbyMemberData.Data.lobby)
            {
                onIsLobbyOwnerStatus?.Invoke(updatedLobby.Owner.user == m_SteamLobbyMemberData.Data.user);
            }
        }
    }
}
#endif