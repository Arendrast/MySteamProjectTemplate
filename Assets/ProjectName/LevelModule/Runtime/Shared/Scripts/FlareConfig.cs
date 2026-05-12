using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts
{
    [Serializable]
    public class FlareConfig
    {
        [field: SerializeField] public float FadeSpeed { get; private set; } = 3f;
        [field: SerializeField] public float Strength { get; private set; } = 1f;

        [Button]
        public void AppointFromScene()
        {
            FadeSpeed = RenderSettings.flareFadeSpeed;
            Strength = RenderSettings.flareStrength;
        }
    }
}