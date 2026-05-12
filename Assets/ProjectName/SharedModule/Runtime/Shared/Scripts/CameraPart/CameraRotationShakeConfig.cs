using System;
using DG.Tweening;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    [Serializable]
    public class CameraRotationShakeConfig
    {
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public float Strength { get; private set; } = 90f;
        [field: SerializeField] public int Vibrato { get; private set; } = 10;
        [field: SerializeField] public float Randomness { get; private set; } = 90f;
        [field: SerializeField] public bool FadeOut { get; private set; } = true;
        [field: SerializeField] public ShakeRandomnessMode ShakeRandomnessMode { get; private set; } = ShakeRandomnessMode.Full;
    }
}