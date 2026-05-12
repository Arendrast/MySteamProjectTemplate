using UnityEngine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.Operator
{
    public class OperatorSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public OperatorMovementConfig MovementConfig { get; private set; }
    }
}