using System;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Operator
{
    [Serializable]
    public class OperatorMovementConfig
    {
        [field: SerializeField] public float AccelerationPerSecond { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }
    }
}