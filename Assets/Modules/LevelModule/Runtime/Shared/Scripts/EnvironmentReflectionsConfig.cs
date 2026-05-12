using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace Modules.LevelModule.Runtime.Shared.Scripts
{
    [Serializable]
    public class EnvironmentReflectionsConfig
    {
        [field: SerializeField] public DefaultReflectionMode DefaultMode { get; private set; } = DefaultReflectionMode.Skybox;
        
        [field: ValueDropdown(nameof(GetDefaultReflectionResolutions))]
        [field: SerializeField] public int DefaultResolution { get; private set; } = 128;
        
        [field: SerializeField] public float IntensityMultiplier { get; private set; } = 1f;
        [field: SerializeField] public int Bounces { get; private set; } = 1;
        
        private IEnumerable<int> GetDefaultReflectionResolutions()
        {
            return new List<int>()
            {
                16,
                32,
                64,
                128,
                256,
                512,
                1024,
                2048
            };
        }
        
        [Button]
        public void AppointFromScene()
        {
            DefaultMode = RenderSettings.defaultReflectionMode;
            DefaultResolution = RenderSettings.defaultReflectionResolution;
            IntensityMultiplier = RenderSettings.reflectionIntensity;
            Bounces = RenderSettings.reflectionBounces;
        }
    }
}