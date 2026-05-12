#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using UnityEngine;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [ModularComponent(typeof(SteamAchievementData), "Names", nameof(label))]
    [AddComponentMenu("")]
    [RequireComponent(typeof(SteamAchievementData))]
    public class SteamAchievementName : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI label;
        private SteamAchievementData m_data;

        private void Awake()
        {
            m_data = GetComponent<SteamAchievementData>();

            if (!string.IsNullOrEmpty(m_data.apiName))
            {
                if (App.Initialized)
                    label.text = StatsAndAchievements.Client.GetAchievementDisplayAttribute(m_data.apiName, AchievementAttributes.name);
                else
                    App.onSteamInitialized.AddListener(Refresh);
            }
        }

        public void Refresh()
        {
            if (!string.IsNullOrEmpty(m_data.apiName))
                label.text = StatsAndAchievements.Client.GetAchievementDisplayAttribute(m_data.apiName, AchievementAttributes.name);

            App.onSteamInitialized.RemoveListener(Refresh);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SteamAchievementName), true)]
    public class SteamAchievementNameEditor : UnityEditor.Editor
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