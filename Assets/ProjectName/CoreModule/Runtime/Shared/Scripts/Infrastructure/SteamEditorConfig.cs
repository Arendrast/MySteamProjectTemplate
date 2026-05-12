using UnityEngine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    [CreateAssetMenu(
        fileName = nameof(SteamEditorConfig),
        menuName = "Configs/Infrastructure/" + nameof(SteamEditorConfig))]
    public class SteamEditorConfig : ScriptableObject
    {
        [field: SerializeField, Tooltip("Enable Steam integration when running in the Unity Editor")]
        public bool UseSteamInEditor { get; private set; }

        public bool ShouldUseSteam =>
#if UNITY_EDITOR
            UseSteamInEditor;
#else
            true;
#endif
    }
}
