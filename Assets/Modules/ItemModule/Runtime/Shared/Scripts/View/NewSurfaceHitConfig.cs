using System.Collections.Generic;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.ItemModule.Runtime.Shared.Scripts.View
{
    [ConfigScope(nameof(NewSurfaceHitConfig))]
    [CreateAssetMenu(fileName = nameof(NewSurfaceHitConfig),
        menuName = "Configs/ItemModule/View/" + nameof(NewSurfaceHitConfig))]
    public class NewSurfaceHitConfig : SerializedScriptableObject
    {
        [field: SerializeField]
        public IReadOnlyDictionary<SurfaceType, IReadOnlyList<AssetReference>>
            SurfaceHitTraceDecalAssetReferencesByType { get; private set; } =
            new Dictionary<SurfaceType, IReadOnlyList<AssetReference>>();

        [field: SerializeField]
        public IReadOnlyDictionary<SurfaceType, AssetReference>
            SurfaceHitTraceEffectAssetReferencesByType { get; private set; }
            = new Dictionary<SurfaceType, AssetReference>();
    }
}