using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Operator
{
    public class OperatorSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public OperatorMovementConfig MovementConfig { get; private set; }
    }
}