using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    [ConfigScope(nameof(CharacterControllerExplodablesConfig))]
    [CreateAssetMenu(fileName = nameof(CharacterControllerExplodablesConfig),
        menuName = "Configs/" + nameof(CharacterControllerExplodablesConfig))]
    public class CharacterControllerExplodablesConfig : ScriptableObject
    {
        [field: SerializeField] public AnimationCurve SpeedCurve { get; private set; }
        [field: SerializeField] public float TimeMultiplier { get; private set; }
        [field: SerializeField] public float MinimumThrowTime { get; private set; } = 0.2f;
    }
}