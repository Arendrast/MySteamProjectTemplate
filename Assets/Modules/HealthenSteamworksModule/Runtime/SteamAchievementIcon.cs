#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using UnityEngine;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [ModularComponent(typeof(SteamAchievementData), "Icons", nameof(image))]
    [AddComponentMenu("")]
    [RequireComponent(typeof(SteamAchievementData))]
    public class SteamAchievementIcon : MonoBehaviour
    {
        public UnityEngine.UI.RawImage image;
        private SteamAchievementData m_data;

        private void Awake()
        {
            m_data = GetComponent<SteamAchievementData>();
        }

        private void Start()
        {
            StatsAndAchievements.Client.OnAchievementStatusChanged.AddListener(HandleChange);

            if (!string.IsNullOrEmpty(m_data.apiName))
            {
                if (App.Initialized)
                {
                    Refresh();
                }
                else
                    App.onSteamInitialized.AddListener(Refresh);
            }
        }

        private void OnDestroy()
        {
            StatsAndAchievements.Client.OnAchievementStatusChanged.RemoveListener(HandleChange);
        }

        private void HandleChange(string arg0, bool arg1)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (!string.IsNullOrEmpty(m_data.apiName))
            {
                StatsAndAchievements.Client.GetAchievementIcon(m_data.apiName, texture =>
                {
                    image.texture = texture;
                });
            }

            App.onSteamInitialized.RemoveListener(Refresh);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SteamAchievementIcon), true)]
    public class SteamAchievementIconEditor : UnityEditor.Editor
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