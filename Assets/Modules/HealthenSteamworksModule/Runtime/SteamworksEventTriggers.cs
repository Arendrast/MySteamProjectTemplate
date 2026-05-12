#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)

using System;
using System.Collections.Generic;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [DisallowMultipleComponent]
    public class SteamworksEventTriggers : MonoBehaviour
    {
        [SerializeField]
        private List<SteamEventTriggerType> m_Delegates;

        private void Awake()
        {
            App.onSteamInitialized.AddListener(initSuccess.Invoke);
            App.onSteamInitializationError.AddListener(initFailed.Invoke);

            App.Client.OnDlcInstalled.AddListener(dlcInstalled.Invoke);
            App.Client.OnNewUrlLaunchParameters.AddListener(newUrlLaunchParameters.Invoke);
            App.Client.OnServersDisconnected.AddListener(serversDisconnected.Invoke);
            App.Client.OnServersConnected.AddListener(serversConnected.Invoke);
            App.Client.OnServersConnectFailure.AddListener(serversConnectFailure.Invoke);

            BigPicture.Client.OnGamepadTextInputShown.AddListener(gamepadTextInputShown.Invoke);
            BigPicture.Client.OnGamepadTextInputDismissed.AddListener(gamepadTextInputDismissed.Invoke);

            Clans.Client.OnGameConnectedChatLeave.AddListener(gameConnectedChatLeave.Invoke);
            Clans.Client.OnChatMessageReceived.AddListener(chatMessageReceived.Invoke);
            Clans.Client.OnGameConnectedChatJoin.AddListener(gameConnectedChatJoin.Invoke);

            Friends.Client.OnGameConnectedFriendChatMsg.AddListener(gameConnectedFriendChatMsg.Invoke);
            Friends.Client.OnFriendRichPresenceUpdate.AddListener(friendRichPresenceUpdate.Invoke);
            Friends.Client.OnPersonaStateChange.AddListener(personaStateChange.Invoke);

            Input.Client.onInputDataChanged.AddListener(inputDataChanged.Invoke);

            Inventory.Client.OnSteamInventoryDefinitionUpdate.AddListener(inventoryDefinitionUpdate.Invoke);
            Inventory.Client.OnSteamInventoryResultReady.AddListener(inventoryResultReady.Invoke);
            Inventory.Client.OnSteamMicroTransactionAuthorizationResponse.AddListener(microTransactionAuthorizationResponse.Invoke);

            Matchmaking.Client.OnFavoritesListChanged.AddListener(serverFavoritesListChanged.Invoke);
            Matchmaking.Client.OnLobbyAskedToLeave.AddListener(lobbyAskedToLeave.Invoke);
            Matchmaking.Client.OnLobbyAuthenticationRequest.AddListener(lobbyAuthenticationRequest.Invoke);
            Matchmaking.Client.OnLobbyChatMsg.AddListener(lobbyChatMsg.Invoke);
            Matchmaking.Client.OnLobbyChatUpdate.AddListener(lobbyChatUpdate.Invoke);
            Matchmaking.Client.OnLobbyDataUpdate.AddListener(lobbyChatDataUpdate.Invoke);
            Matchmaking.Client.OnLobbyEnterFailed.AddListener(lobbyEnterFailed.Invoke);
            Matchmaking.Client.OnLobbyEnterSuccess.AddListener(lobbyEnterSuccess.Invoke);
            Matchmaking.Client.OnLobbyGameCreated.AddListener(lobbyGameCreated.Invoke);
            Matchmaking.Client.OnLobbyInvite.AddListener(lobbyInvite.Invoke);
            Matchmaking.Client.OnLobbyLeave.AddListener(lobbyLeave.Invoke);

            Overlay.Client.OnGameLobbyJoinRequested.AddListener(gameLobbyJoinRequested.Invoke);
            Overlay.Client.OnGameOverlayActivated.AddListener(gameOverlayActivated.Invoke);
            Overlay.Client.OnGameRichPresenceJoinRequested.AddListener(gameRichPresenceJoinRequested.Invoke);
            Overlay.Client.OnGameServerChangeRequested.AddListener(gameServerChangeRequested.Invoke);

            Parties.Client.OnActiveBeaconsUpdated.AddListener(activeBeaconsUpdated.Invoke);
            Parties.Client.OnAvailableBeaconLocationsUpdated.AddListener(availableBeaconLocationsUpdated.Invoke);
            Parties.Client.OnReservationNotificationCallback.AddListener(reservationNotificationCallback.Invoke);

            RemotePlay.Client.OnSessionConnected.AddListener(remotePlaySessionConnected.Invoke);
            RemotePlay.Client.OnSessionDisconnected.AddListener(remotePlaySessionDisconnected.Invoke);

            RemoteStorage.Client.OnLocalFileChange.AddListener(remoteStorageFileChange.Invoke);

            Screenshots.Client.OnScreenshotReady.AddListener(screenshotReady.Invoke);
            Screenshots.Client.OnScreenshotRequested.AddListener(screenshotRequested.Invoke);

            StatsAndAchievements.Client.OnUserAchievementStored.AddListener(achievementStored.Invoke);
            StatsAndAchievements.Client.OnUserStatsReceived.AddListener(statsReceived.Invoke);
            StatsAndAchievements.Client.OnUserStatsStored.AddListener(statsStored.Invoke);
            StatsAndAchievements.Client.OnUserStatsUnloaded.AddListener(statsUnloaded.Invoke);

            Utilities.Client.OnAppResumeFromSuspend.AddListener(appResumeFromSuspend.Invoke);
            Utilities.Client.OnKeyboardClosed.AddListener(keyboardClosed.Invoke);
            Utilities.Client.OnKeyboardShown.AddListener(keyboardShown.Invoke);
        }

        private void OnDestroy()
        {
            App.onSteamInitialized.RemoveListener(initSuccess.Invoke);
            App.onSteamInitializationError.RemoveListener(initFailed.Invoke);

            App.Client.OnDlcInstalled.RemoveListener(dlcInstalled.Invoke);
            App.Client.OnNewUrlLaunchParameters.RemoveListener(newUrlLaunchParameters.Invoke);
            App.Client.OnServersDisconnected.RemoveListener(serversDisconnected.Invoke);
            App.Client.OnServersConnected.RemoveListener(serversConnected.Invoke);
            App.Client.OnServersConnectFailure.RemoveListener(serversConnectFailure.Invoke);

            BigPicture.Client.OnGamepadTextInputShown.RemoveListener(gamepadTextInputShown.Invoke);
            BigPicture.Client.OnGamepadTextInputDismissed.RemoveListener(gamepadTextInputDismissed.Invoke);

            Clans.Client.OnGameConnectedChatLeave.RemoveListener(gameConnectedChatLeave.Invoke);
            Clans.Client.OnChatMessageReceived.RemoveListener(chatMessageReceived.Invoke);
            Clans.Client.OnGameConnectedChatJoin.RemoveListener(gameConnectedChatJoin.Invoke);

            Friends.Client.OnGameConnectedFriendChatMsg.RemoveListener(gameConnectedFriendChatMsg.Invoke);
            Friends.Client.OnFriendRichPresenceUpdate.RemoveListener(friendRichPresenceUpdate.Invoke);
            Friends.Client.OnPersonaStateChange.RemoveListener(personaStateChange.Invoke);

            Input.Client.onInputDataChanged.RemoveListener(inputDataChanged.Invoke);

            Inventory.Client.OnSteamInventoryDefinitionUpdate.RemoveListener(inventoryDefinitionUpdate.Invoke);
            Inventory.Client.OnSteamInventoryResultReady.RemoveListener(inventoryResultReady.Invoke);
            Inventory.Client.OnSteamMicroTransactionAuthorizationResponse.RemoveListener(microTransactionAuthorizationResponse.Invoke);

            Matchmaking.Client.OnFavoritesListChanged.RemoveListener(serverFavoritesListChanged.Invoke);
            Matchmaking.Client.OnLobbyAskedToLeave.RemoveListener(lobbyAskedToLeave.Invoke);
            Matchmaking.Client.OnLobbyAuthenticationRequest.RemoveListener(lobbyAuthenticationRequest.Invoke);
            Matchmaking.Client.OnLobbyChatMsg.RemoveListener(lobbyChatMsg.Invoke);
            Matchmaking.Client.OnLobbyChatUpdate.RemoveListener(lobbyChatUpdate.Invoke);
            Matchmaking.Client.OnLobbyDataUpdate.RemoveListener(lobbyChatDataUpdate.Invoke);
            Matchmaking.Client.OnLobbyEnterFailed.RemoveListener(lobbyEnterFailed.Invoke);
            Matchmaking.Client.OnLobbyEnterSuccess.RemoveListener(lobbyEnterSuccess.Invoke);
            Matchmaking.Client.OnLobbyGameCreated.RemoveListener(lobbyGameCreated.Invoke);
            Matchmaking.Client.OnLobbyInvite.RemoveListener(lobbyInvite.Invoke);
            Matchmaking.Client.OnLobbyLeave.RemoveListener(lobbyLeave.Invoke);

            Overlay.Client.OnGameLobbyJoinRequested.RemoveListener(gameLobbyJoinRequested.Invoke);
            Overlay.Client.OnGameOverlayActivated.RemoveListener(gameOverlayActivated.Invoke);
            Overlay.Client.OnGameRichPresenceJoinRequested.RemoveListener(gameRichPresenceJoinRequested.Invoke);
            Overlay.Client.OnGameServerChangeRequested.RemoveListener(gameServerChangeRequested.Invoke);

            Parties.Client.OnActiveBeaconsUpdated.RemoveListener(activeBeaconsUpdated.Invoke);
            Parties.Client.OnAvailableBeaconLocationsUpdated.RemoveListener(availableBeaconLocationsUpdated.Invoke);
            Parties.Client.OnReservationNotificationCallback.RemoveListener(reservationNotificationCallback.Invoke);

            RemotePlay.Client.OnSessionConnected.RemoveListener(remotePlaySessionConnected.Invoke);
            RemotePlay.Client.OnSessionDisconnected.RemoveListener(remotePlaySessionDisconnected.Invoke);

            RemoteStorage.Client.OnLocalFileChange.RemoveListener(remoteStorageFileChange.Invoke);

            Screenshots.Client.OnScreenshotReady.RemoveListener(screenshotReady.Invoke);
            Screenshots.Client.OnScreenshotRequested.RemoveListener(screenshotRequested.Invoke);

            StatsAndAchievements.Client.OnUserAchievementStored.RemoveListener(achievementStored.Invoke);
            StatsAndAchievements.Client.OnUserStatsReceived.RemoveListener(statsReceived.Invoke);
            StatsAndAchievements.Client.OnUserStatsStored.RemoveListener(statsStored.Invoke);
            StatsAndAchievements.Client.OnUserStatsUnloaded.RemoveListener(statsUnloaded.Invoke);

            Utilities.Client.OnAppResumeFromSuspend.RemoveListener(appResumeFromSuspend.Invoke);
            Utilities.Client.OnKeyboardClosed.RemoveListener(keyboardClosed.Invoke);
            Utilities.Client.OnKeyboardShown.RemoveListener(keyboardShown.Invoke);
        }

        public UnityEvent initSuccess;
        public StringEvent initFailed;
        public DlcInstalledEvent dlcInstalled;
        public UnityEvent newUrlLaunchParameters;
        public App.Client.UnityEventServersDisconnected serversDisconnected;
        public UnityEvent serversConnected;
        public App.Client.UnityEventServersConnectFailure serversConnectFailure;
        public UnityEvent gamepadTextInputShown;
        public StringEvent gamepadTextInputDismissed;
        public GameConnectedClanChatMsgEvent chatMessageReceived;
        public GameConnectedChatJoinEvent gameConnectedChatJoin;
        public GameConnectedChatLeaveEvent gameConnectedChatLeave;
        public GameConnectedFriendChatMsgEvent gameConnectedFriendChatMsg;
        public FriendRichPresenceUpdateEvent friendRichPresenceUpdate;
        public PersonaStateChangeEvent personaStateChange;
        public ControllerDataEvent inputDataChanged;
        public SteamInventoryDefinitionUpdateEvent inventoryDefinitionUpdate;
        public SteamInventoryResultReadyEvent inventoryResultReady;
        public SteamMicroTransactionAuthorizationResponce microTransactionAuthorizationResponse;
        public FavoritesListChangedEvent serverFavoritesListChanged;
        public LobbyDataEvent lobbyAskedToLeave;
        public LobbyAuthenticationEvent lobbyAuthenticationRequest;
        public LobbyChatMsgEvent lobbyChatMsg;
        public LobbyChatUpdateEvent lobbyChatUpdate;
        public LobbyDataUpdateEvent lobbyChatDataUpdate;
        public LobbyEnterEvent lobbyEnterFailed;
        public LobbyEnterEvent lobbyEnterSuccess;
        public LobbyGameCreatedEvent lobbyGameCreated;
        public LobbyInviteEvent lobbyInvite;
        public LobbyDataEvent lobbyLeave;
        public GameLobbyJoinRequestedEvent gameLobbyJoinRequested;
        public GameOverlayActivatedEvent gameOverlayActivated;
        public GameRichPresenceJoinRequestedEvent gameRichPresenceJoinRequested;
        public GameServerChangeRequestedEvent gameServerChangeRequested;
        public ActiveBeaconsUpdatedEvent activeBeaconsUpdated;
        public AvailableBeaconLocationsUpdatedEvent availableBeaconLocationsUpdated;
        public ReservationNotificationCallbackEvent reservationNotificationCallback;
        public SteamRemotePlaySessionConnectedEvent remotePlaySessionConnected;
        public SteamRemotePlaySessionDisconnectedEvent remotePlaySessionDisconnected;
        public RemoteStorageLocalFileChangeEvent remoteStorageFileChange;
        public ScreenshotReadyEvent screenshotReady;
        public UnityEvent screenshotRequested;
        public UserAchievementStoredEvent achievementStored;
        public UserStatsReceivedEvent statsReceived;
        public UserStatsStoredEvent statsStored;
        public UserStatsUnloadedEvent statsUnloaded;
        public UnityEvent appResumeFromSuspend;
        public UnityEvent keyboardClosed;
        public UnityEvent keyboardShown;
    }

    public enum SteamEventTriggerType
    {
        AchievementStored,
        ActiveBeaconsUpdated,
        AppResumeFromSuspend,
        AvailableBeaconLocationsUpdated,
        ChatMessageReceived,
        DlcInstalled,
        FriendRichPresenceUpdate,
        GameConnectedChatJoin,
        GameConnectedChatLeave,
        GameConnectedFriendChatMsg,
        GamepadTextInputDismissed,
        GamepadTextShown,
        InitializationError,
        InitializationSuccess,        
        InputDataChanged,
        InventoryDefinitionUpdate,
        InventoryResultReady,
        KeyboardClosed,
        KeyboardShown,
        LobbyAskedToLeave,
        LobbyAuthenticationRequest,
        LobbyChatMsg,
        LobbyChatUpdate,
        LobbyDataUpdate,
        LobbyEnterFailed,
        LobbyEnterSuccess,
        LobbyGameCreated,
        LobbyInvite,
        LobbyJoinRequested,
        LobbyLeave,
        MicroTransactionAuthorizationResponse,
        NewUrlLaunchParameters,   
        OverlayActivated,
        PersonaStateChange,
        RemotePlaySessionConnected,
        RemotePlaySessionDisconnected,
        RemoteStorageFileChanged,
        ReservationNotificationCallback,
        RichPresenceJoinRequested,
        ScreenshotReady,
        ScreenshotRequested,
        ServerChangeRequested,
        ServersConnectFailure,
        ServersConnected,
        ServersDisconnected,
        ServerFavoritesListChanged,
        StatsReceived,
        StatsStored,
        StatsUnloaded,
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SteamworksEventTriggers), true)]
    public class SteamworksEventTriggersEditor : Editor
    {
        SerializedProperty m_DelegatesProperty;

        GUIContent m_IconToolbarMinus;
        GUIContent m_EventIDName;
        GUIContent[] m_EventTypes;
        GUIContent m_AddButtonContent;

        protected virtual void OnEnable()
        {
            m_DelegatesProperty = serializedObject.FindProperty("m_Delegates");
            m_AddButtonContent = new GUIContent("Add New Event Type");
            m_EventIDName = new GUIContent("");
            // Have to create a copy since otherwise the tooltip will be overwritten.
            m_IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            m_IconToolbarMinus.tooltip = "Remove all events in this list.";

            string[] eventNames = Enum.GetNames(typeof(SteamEventTriggerType));
            m_EventTypes = new GUIContent[eventNames.Length];
            for (int i = 0; i < eventNames.Length; ++i)
            {
                m_EventTypes[i] = new GUIContent(ObjectNames.NicifyVariableName(eventNames[i]));
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            int toBeRemovedEntry = -1;

            EditorGUILayout.Space();

            Vector2 removeButtonSize = GUIStyle.none.CalcSize(m_IconToolbarMinus);

            for (int i = 0; i < m_DelegatesProperty.arraySize; ++i)
            {
                SerializedProperty delegateProperty = m_DelegatesProperty.GetArrayElementAtIndex(i);
                //SerializedProperty eventProperty = delegateProperty.FindPropertyRelative("eventID");
                //SerializedProperty callbacksProperty = delegateProperty.FindPropertyRelative("callback");
                m_EventIDName.text = delegateProperty.enumDisplayNames[delegateProperty.enumValueIndex];

                switch ((SteamEventTriggerType)delegateProperty.enumValueIndex)
                {
                    case SteamEventTriggerType.AchievementStored:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.achievementStored)), m_EventIDName);
                        break;
                    case SteamEventTriggerType.ActiveBeaconsUpdated:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.activeBeaconsUpdated)), m_EventIDName);
                        break;
                    case SteamEventTriggerType.AppResumeFromSuspend:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.appResumeFromSuspend)), m_EventIDName);
                        break;
                    case SteamEventTriggerType.AvailableBeaconLocationsUpdated:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.availableBeaconLocationsUpdated)), m_EventIDName);
                        break;
                    case SteamEventTriggerType.ChatMessageReceived:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.chatMessageReceived)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.DlcInstalled:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.dlcInstalled)), m_EventIDName);
                        break;
                    case SteamEventTriggerType.FriendRichPresenceUpdate:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.friendRichPresenceUpdate)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.GameConnectedChatJoin:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.gameConnectedChatJoin)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.GameConnectedChatLeave:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.gameConnectedChatLeave)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.GameConnectedFriendChatMsg:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.gameConnectedFriendChatMsg)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.GamepadTextInputDismissed:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.gamepadTextInputDismissed)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.GamepadTextShown:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.gamepadTextInputShown)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.InputDataChanged:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.inputDataChanged)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.InventoryDefinitionUpdate:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.inventoryDefinitionUpdate)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.InventoryResultReady:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.inventoryResultReady)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.KeyboardClosed:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.keyboardClosed)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.KeyboardShown:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.keyboardShown)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyAskedToLeave:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.lobbyAskedToLeave)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyAuthenticationRequest:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.lobbyAuthenticationRequest)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyChatMsg:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.lobbyChatMsg)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyChatUpdate:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.lobbyChatUpdate)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyDataUpdate:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.lobbyChatDataUpdate)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyEnterFailed:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.lobbyEnterFailed)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyEnterSuccess:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.lobbyEnterSuccess)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyGameCreated:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.lobbyGameCreated)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyInvite:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.lobbyInvite)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyJoinRequested:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.gameLobbyJoinRequested)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.LobbyLeave:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.lobbyLeave)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.MicroTransactionAuthorizationResponse:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.microTransactionAuthorizationResponse)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.NewUrlLaunchParameters:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.newUrlLaunchParameters)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.OverlayActivated:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.gameOverlayActivated)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.PersonaStateChange:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.personaStateChange)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.RemotePlaySessionConnected:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.remotePlaySessionConnected)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.RemotePlaySessionDisconnected:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.remotePlaySessionDisconnected)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.RemoteStorageFileChanged:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.remoteStorageFileChange)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.ReservationNotificationCallback:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.reservationNotificationCallback)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.RichPresenceJoinRequested:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.gameRichPresenceJoinRequested)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.ScreenshotReady:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.screenshotReady)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.ScreenshotRequested:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.screenshotRequested)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.ServerChangeRequested:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.gameServerChangeRequested)), m_EventIDName); break;
                    case SteamEventTriggerType.ServerFavoritesListChanged:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.serverFavoritesListChanged)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.ServersConnected:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.serversConnected)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.ServersConnectFailure:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.serversConnectFailure)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.ServersDisconnected:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.serversDisconnected)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.StatsReceived:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.statsReceived)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.StatsStored:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.statsStored)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.StatsUnloaded:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.statsUnloaded)), m_EventIDName); 
                        break;
                    case SteamEventTriggerType.InitializationError:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.initFailed)), m_EventIDName);
                        break;
                        case SteamEventTriggerType.InitializationSuccess:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SteamworksEventTriggers.initSuccess)), m_EventIDName);
                        break;
                }
                
                Rect callbackRect = GUILayoutUtility.GetLastRect();

                Rect removeButtonPos = new Rect(callbackRect.xMax - removeButtonSize.x - 8, callbackRect.y + 1, removeButtonSize.x, removeButtonSize.y);
                if (GUI.Button(removeButtonPos, m_IconToolbarMinus, GUIStyle.none))
                {
                    toBeRemovedEntry = i;
                }

                EditorGUILayout.Space();
            }

            if (toBeRemovedEntry > -1)
            {
                RemoveEntry(toBeRemovedEntry);
            }

            Rect btPosition = GUILayoutUtility.GetRect(m_AddButtonContent, GUI.skin.button);
            const float addButtonWidth = 200f;
            btPosition.x = btPosition.x + (btPosition.width - addButtonWidth) / 2;
            btPosition.width = addButtonWidth;
            if (GUI.Button(btPosition, m_AddButtonContent))
            {
                ShowAddTriggerMenu();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void RemoveEntry(int toBeRemovedEntry)
        {
            m_DelegatesProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
        }

        void ShowAddTriggerMenu()
        {
            // Now create the menu, add items and show it
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < m_EventTypes.Length; ++i)
            {
                bool active = true;

                // Check if we already have a Entry for the current eventType, if so, disable it
                for (int p = 0; p < m_DelegatesProperty.arraySize; ++p)
                {
                    SerializedProperty delegateEntry = m_DelegatesProperty.GetArrayElementAtIndex(p);
                    //SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("eventID");
                    if (delegateEntry.enumValueIndex == i)
                    {
                        active = false;
                    }
                }
                if (active)
                    menu.AddItem(m_EventTypes[i], false, OnAddNewSelected, i);
                else
                    menu.AddDisabledItem(m_EventTypes[i]);
            }
            menu.ShowAsContext();
            Event.current.Use();
        }

        private void OnAddNewSelected(object index)
        {
            int selected = (int)index;

            m_DelegatesProperty.arraySize += 1;
            SerializedProperty delegateEntry = m_DelegatesProperty.GetArrayElementAtIndex(m_DelegatesProperty.arraySize - 1);
            //SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("eventID");
            delegateEntry.enumValueIndex = selected;
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
#endif