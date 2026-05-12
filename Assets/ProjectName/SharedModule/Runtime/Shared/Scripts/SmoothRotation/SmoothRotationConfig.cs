using System;
using Animancer.Units;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.SmoothRotation
{
    [Serializable]
    public class SmoothRotationConfig
    {
        [field: SerializeField] public Transform RotatableTransform { get; private set; }
        
        [field: SerializeField] 
        [field: DegreesPerSecond] public float RotationSpeedInDegreesPerSecond { get; private set; } = 40;
        
        [field: SerializeField] public RotationLimitationsConfig RotationLimitationsConfig { get; private set; }
    }
}