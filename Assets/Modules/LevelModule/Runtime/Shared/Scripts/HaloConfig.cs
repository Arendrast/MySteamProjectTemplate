using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.LevelModule.Runtime.Shared.Scripts
{
    [Serializable]
    public class HaloConfig
    {
        [field: SerializeField] public float Strength { get; private set; } = 0.5f;

        [Button]
        public void AppointFromScene()
        {
            Strength = RenderSettings.haloStrength;
        }
    }
}