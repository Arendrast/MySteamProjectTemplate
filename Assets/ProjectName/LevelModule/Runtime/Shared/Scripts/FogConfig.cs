using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts
{
    [Serializable]
    public class FogConfig
    {
        [field: SerializeField] public bool Enable { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public FogMode Mode { get; private set; }
       
        [field: ShowIf(nameof(Mode), FogMode.Linear)]
        [field: SerializeField] public float Start { get; private set; }
        [field: ShowIf(nameof(Mode), FogMode.Linear)]
        [field: SerializeField] public float End { get; private set; }
        
        [Button]
        public void AppointFromScene()
        {
            Enable = RenderSettings.fog;
            Color = RenderSettings.fogColor;
            Mode = RenderSettings.fogMode;
            Start = RenderSettings.fogStartDistance;
            End = RenderSettings.fogEndDistance;
        }
    }
}