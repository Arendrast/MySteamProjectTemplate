using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.LevelModule.Runtime.Shared.Scripts
{
    [ConfigScope(nameof(LevelsConfig))]
    [CreateAssetMenu(fileName = nameof(LevelsConfig), menuName = "Configs/" + nameof(LevelsConfig))]
    public class LevelsConfig : SerializedScriptableObject
    {
        [field: SerializeField] public LevelConfig[] LevelsConfigs { get; private set; }
    }
}
