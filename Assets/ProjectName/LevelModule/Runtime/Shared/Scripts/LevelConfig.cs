using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts
{
    [CreateAssetMenu(fileName = nameof(LevelConfig), menuName = "Configs/" + nameof(LevelConfig))]
    public class LevelConfig : SerializedScriptableObject
    {
        [field: SerializeField] public string LevelName { get; private set; }
        [field: SerializeField] public string SceneName { get; private set; }
        [field: SerializeField] public int StartLevelIndex { get; private set; }
    }
}