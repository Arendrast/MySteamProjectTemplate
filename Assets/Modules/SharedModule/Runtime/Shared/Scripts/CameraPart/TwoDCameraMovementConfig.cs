using System;
using Animancer.Units;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    [Serializable]
    public class TwoDCameraMovementConfig
    {
        [field: SerializeField] [field: MetersPerSecondPerSecond] public float AccelerationPerSecond { get; private set; }
        [field: SerializeField] [field: MetersPerSecond] public float MaxSpeed { get; private set; }
        [field: SerializeField] [field: Meters] public float MaxDeltaOffset { get; private set; }
    }
}