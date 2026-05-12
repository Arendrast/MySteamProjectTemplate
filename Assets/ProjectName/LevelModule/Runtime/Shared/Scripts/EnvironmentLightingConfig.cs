using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts
{
    [Serializable]
    public class EnvironmentLightingConfig
    {
        [field: SerializeField] public AmbientMode Source { get; private set; } = AmbientMode.Skybox;
        
        [field: ShowIf(nameof(Source), AmbientMode.Flat)]
        [field: SerializeField] public Color AmbientColor { get; private set; } = new Color(54, 58, 66, 1);
        
        [field: ShowIf(nameof(Source), AmbientMode.Trilight)]
        [field: ColorUsage(true, true)]
        [field: SerializeField] public Color AmbientSkyColor { get; private set; } = new Color(199, 199, 199);
        [field: ShowIf(nameof(Source), AmbientMode.Trilight)]
        [field: ColorUsage(true, true)]
        [field: SerializeField] public Color EquatorColor { get; private set; } = new Color(175, 161, 153);
        [field: ShowIf(nameof(Source), AmbientMode.Trilight)]
        [field: ColorUsage(true, true)]
        [field: SerializeField] public Color GroundColor { get; private set; } = new Color(18, 18, 18);
        
        [field: ShowIf(nameof(Source), AmbientMode.Skybox)]
        [field: SerializeField] public float AmbientIntensity { get; private set; } = 1f;

        [Button]
        public void AppointFromScene()
        {
            Source = RenderSettings.ambientMode;
            AmbientIntensity = RenderSettings.ambientIntensity;
            AmbientSkyColor = RenderSettings.ambientSkyColor;
            EquatorColor = RenderSettings.ambientEquatorColor;
            GroundColor = RenderSettings.ambientGroundColor;
            AmbientColor = RenderSettings.ambientLight;
        }
    }
}