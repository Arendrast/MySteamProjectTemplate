#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System.Collections.Generic;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Events;
using UnityEngine;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [HelpURL("")]
    public class SteamInputManager : MonoBehaviour
    {
        public static SteamInputManager current;

        [Tooltip("If set to true then we will attempt to force Steam to use input for this app on start.\nThis is generally only needed in editor testing.")]
        [SerializeField]
        private bool forceInput = true;
        [Tooltip("If set to true the system will update every input action every frame for every controller found")]
        public bool autoUpdate = true;
        [Header("Events")]
        public ControllerDataEvent onInputDataChanged;

        private bool lastAutoUpdate;

        public static bool AutoUpdate
        {
            get => current != null ? current.autoUpdate : false;
            set 
            { 
                if(current != null)
                    current.autoUpdate = value;
            }
        }

        public static List<InputControllerStateData> Controllers { get; private set; } = new List<InputControllerStateData>();

        private void Start()
        {
            current = this;

            Input.Client.onInputDataChanged.AddListener(onInputDataChanged.Invoke);
            Input.Client.IsAutoRefreshControllerState = autoUpdate;
            lastAutoUpdate = autoUpdate;

            if (!Interface.IsReady)
                Interface.OnReady += HandleInitialization;
            else
                HandleInitialization();
        }

        private void HandleInitialization()
        {
            Interface.OnReady -= HandleInitialization;

            if (forceInput)
            {
                Application.OpenURL($"steam://forceinputappid/{App.Id}");
            }
        }

        private void OnDestroy()
        {
            if(current == this)
                current = null;

            Input.Client.onInputDataChanged.RemoveListener(onInputDataChanged.Invoke);

            if (forceInput)
                Application.OpenURL("steam://forceinputappid/0");
        }

        private void LateUpdate()
        {
            if (!Interface.IsReady)
                return;

            // Sync Unity inspector toggle → API
            if (Input.Client.IsAutoRefreshControllerState != autoUpdate)
                Input.Client.IsAutoRefreshControllerState = autoUpdate;

            // Optionally mirror API → manager (if other code changed it)
            if (lastAutoUpdate != Input.Client.IsAutoRefreshControllerState)
            {
                lastAutoUpdate = Input.Client.IsAutoRefreshControllerState;
                autoUpdate = lastAutoUpdate;
            }

            if (autoUpdate)
                Input.Client.RunFrame();
        }

        public void Refresh() => Input.Client.RunFrame();
    }
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SteamInputManager), true)]
    public class SteamInputManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
#endif