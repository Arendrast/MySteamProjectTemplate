#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using UnityEngine;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [ModularComponent(typeof(SteamAchievementData), "Descriptions", nameof(label))]
    [AddComponentMenu("")]
    [RequireComponent(typeof(SteamAchievementData))]
    public class SteamAchievementDescription : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI label;
        private SteamAchievementData m_data;

        private void Awake()
        {
            m_data = GetComponent<SteamAchievementData>();

            if (!string.IsNullOrEmpty(m_data.apiName))
            {
                if (App.Initialized)
                    label.text = StatsAndAchievements.Client.GetAchievementDisplayAttribute(m_data.apiName, AchievementAttributes.desc);
                else
                    App.onSteamInitialized.AddListener(Refresh);
            }
        }

        public void Refresh()
        {
            if (!string.IsNullOrEmpty(m_data.apiName))
                label.text = StatsAndAchievements.Client.GetAchievementDisplayAttribute(m_data.apiName, AchievementAttributes.desc);

            App.onSteamInitialized.RemoveListener(Refresh);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SteamAchievementDescription), true)]
    public class SteamAchievementDescriptionEditor : UnityEditor.Editor
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