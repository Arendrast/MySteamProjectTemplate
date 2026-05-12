using ProjectName.SharedModule.Runtime.Shared.Scripts.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.ItemModule.Runtime.Shared.Scripts.View
{
    [ConfigScope(nameof(ItemsViewConfig))]
    [CreateAssetMenu(fileName = nameof(ItemsViewConfig), menuName = "Configs/ItemModule/View/" + nameof(ItemsViewConfig))]
    public class ItemsViewConfig : SerializedScriptableObject
    {
        [field: SerializeField] public IItemViewConfig[] ItemConfigs { get; private set; } 
    }
}