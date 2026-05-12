using Animancer.Units;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Configs
{
    [ConfigScope(nameof(MovementConfig))]
    [CreateAssetMenu(fileName = nameof(MovementConfig), menuName = "Configs/Player/" + nameof(MovementConfig))]
    public class MovementConfig : ScriptableObject
    {
        [field: Header("Вращение")] [field: DegreesPerSecond]
        [field: SerializeField, Tooltip("Скорость вращения игрока в градусах в секунду")]
        public float RotationSpeed { get; private set; } = 50;
    }
}