using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts
{
    [CreateAssetMenu(fileName = nameof(LightingConfig), menuName = "Configs/" + nameof(LightingConfig))]
    public class LightingConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReference SkyBoxMaterialReference { get; private set; }
        [field: SerializeField] public bool CanSkyBoxBeNull { get; private set; }
        [field: SerializeField] public Color RealtimeShadowColor { get; private set; }
        [field: SerializeField] public EnvironmentLightingConfig EnvironmentLightingConfig { get; private set; }
        [field: SerializeField] public EnvironmentReflectionsConfig EnvironmentReflectionsConfig { get; private set; }
        [field: SerializeField] public FogConfig FogConfig { get; private set; }
        [field: SerializeField] public FlareConfig FlareConfig { get; private set; }
        [field: SerializeField] public HaloConfig HaloConfig { get; private set; }
    }
}