using System;
using Animancer.Units;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.SmoothRotation
{
    [Serializable]
    public class RotationLimitationsConfig
    {
        [field: SerializeField] public bool Enabled { get; private set; }
        [field: SerializeField] [field: Degrees] public float MinimalDegrees { get; private set; }
        [field: SerializeField] [field: Degrees] public float MaximalDegrees { get; private set; }
    }
}