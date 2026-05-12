#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)

using System;
using System.Collections.Generic;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.ScriptableObjects;
using Steamworks;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [AddComponentMenu("Steamworks/User")]
    public class SteamUserData : MonoBehaviour, ISteamUserData
    {
        public enum ManagedEvents
        {
            Changed,
            Clicked,
        }

        public bool localUser = false;

        public UserData Data
        {
            get => m_Data;
            set
            {
                m_Data = value;
                onChanged?.Invoke(new PersonaStateChange_t { m_nChangeFlags = (EPersonaChange)int.MaxValue, m_ulSteamID = value });
            }
        }

        [HideInInspector]
        public PersonaStateChangeEvent onChanged;

        private UserData m_Data;
        [SerializeField]
        private List<string> m_Delegates;

        private void Awake()
        {
            Friends.Client.OnPersonaStateChange.AddListener(GlobalPersonaUpdate);
        }

        private void Start()
        {
            if (!App.Initialized)
                App.onSteamInitialized.AddListener(HandleInitialization);
            else
                if (localUser) Data = UserData.Me;
        }

        private void HandleInitialization()
        {
            if (localUser) Data = UserData.Me;
            App.onSteamInitialized.RemoveListener(HandleInitialization);
        }

        private void OnDestroy()
        {
            Friends.Client.OnPersonaStateChange.RemoveListener(GlobalPersonaUpdate);
            App.onSteamInitialized.RemoveListener(HandleInitialization);
        }

        private void GlobalPersonaUpdate(PersonaStateChange arg0)
        {
            if (arg0.SubjectId == Data)
                onChanged?.Invoke(arg0);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SteamUserData), true)]
    public class SteamUserDataEditor : ModularEditor
    {
        private SerializedProperty localUserProp;
        private SteamToolsSettings settings;

        protected override Type[] AllowedTypes => new Type[]
        {
            typeof(SteamUserAvatar),
            typeof(SteamUserHexInput),
            typeof(SteamUserHexLabel),
            typeof(SteamUserLevel),
            typeof(SteamUserName),
            typeof(SteamUserStatus),
            typeof(SteamUserLobbyInvite),
            typeof(SteamUserDataEvents),
        };

        private void OnEnable()
        {
            settings = SteamToolsSettings.GetOrCreate();
            localUserProp = serializedObject.FindProperty("localUser");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefault(
                "Project/Player/Steamworks"
                , $"https://partner.steamgames.com/apps/landing/{settings.Get(settings.ActiveApp.Value).applicationId}"
                , "https://kb.heathen.group/steam/features/lobby"
                , "https://discord.gg/heathen-group-463483739612381204"
                , new[] { localUserProp });

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
#endif