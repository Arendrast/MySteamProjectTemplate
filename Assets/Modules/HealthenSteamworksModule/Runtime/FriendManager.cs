#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events;
using Steamworks;
using UnityEngine;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [HelpURL("https://kb.heathen.group/assets/steamworks")]
    [DisallowMultipleComponent]
    public class FriendManager : MonoBehaviour
    {
        public bool ListenForFriendsMessages
        {
            get => Friends.Client.IsListenForFriendsMessages;
            set => Friends.Client.IsListenForFriendsMessages = value;
        }

        public GameConnectedFriendChatMsgEvent evtGameConnectedChatMsg;
        public FriendRichPresenceUpdateEvent evtRichPresenceUpdated;
        public PersonaStateChangeEvent evtPersonaStateChanged;

        private void OnEnable()
        {
            Friends.Client.OnGameConnectedFriendChatMsg.AddListener(evtGameConnectedChatMsg.Invoke);
            Friends.Client.OnFriendRichPresenceUpdate.AddListener(evtRichPresenceUpdated.Invoke);
            Friends.Client.OnPersonaStateChange.AddListener(evtPersonaStateChanged.Invoke);
        }

        private void OnDisable()
        {
            Friends.Client.OnGameConnectedFriendChatMsg.RemoveListener(evtGameConnectedChatMsg.Invoke);
            Friends.Client.OnFriendRichPresenceUpdate.RemoveListener(evtRichPresenceUpdated.Invoke);
            Friends.Client.OnPersonaStateChange.RemoveListener(evtPersonaStateChanged.Invoke);
        }

        public UserData[] GetFriends(EFriendFlags flags) => Friends.Client.GetFriends(flags);
        public UserData[] GetCoplayFriends() => Friends.Client.GetCoplayFriends();
        public string GetFriendMessage(UserData userId, int index, out EChatEntryType type) => Friends.Client.GetFriendMessage(userId, index, out type);
        public bool SendMessage(UserData friend, string message) => Friends.Client.ReplyToFriendMessage(friend, message);
    }
}
#endif