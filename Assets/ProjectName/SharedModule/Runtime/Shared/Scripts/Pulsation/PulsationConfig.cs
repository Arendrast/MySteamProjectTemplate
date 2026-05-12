using System;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Pulsation
{
    [Serializable]
    public class PulsationConfig
    {
        [field: SerializeField] public float PulsationFrequency { get; private set; } = 0.2f;
        [field: SerializeField] public float PulsationValue { get; private set; } = 5;
    }
}