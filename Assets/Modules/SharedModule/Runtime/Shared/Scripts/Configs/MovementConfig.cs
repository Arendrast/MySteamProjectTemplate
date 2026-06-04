using Animancer.Units;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Configs
{
    [ConfigScope(nameof(MovementConfig))]
    [CreateAssetMenu(fileName = nameof(MovementConfig), menuName = "Configs/Player/" + nameof(MovementConfig))]
    public class MovementConfig : ScriptableObject
    {
    }
}