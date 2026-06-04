using System;
using Animancer.Units;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Operator
{
    [Serializable]
    public class OperatorMovementConfig
    {
        [field: SerializeField] [field: MetersPerSecondPerSecond] public float AccelerationPerSecond { get; private set; }
        [field: SerializeField] [field: MetersPerSecond] public float MaxSpeed { get; private set; }
    }
}