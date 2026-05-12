using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.Index;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.ItemModule.Runtime.Shared.Scripts.Logic
{
    [ConfigScope(nameof(ItemsConfig))]
    [CreateAssetMenu(fileName = nameof(ItemsConfig), menuName = "Configs/ItemModule/Logic/" + nameof(ItemsConfig))]
    public class ItemsConfig : SerializedScriptableObject, IIndexableConfigsProvider<IItemConfig>
    {
        [field: SerializeField] public IItemConfig[] Configs { get; private set; } 
    }
}