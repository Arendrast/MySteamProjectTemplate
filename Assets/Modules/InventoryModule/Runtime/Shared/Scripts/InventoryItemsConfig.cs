using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Modules.InventoryModule.Runtime.Shared.Scripts
{
    [ConfigScope(nameof(InventoryItemsConfig))]
    [CreateAssetMenu(fileName = nameof(InventoryItemsConfig), menuName = "Configs/" + nameof(InventoryItemsConfig))]
    public class InventoryItemsConfig : SerializedScriptableObject
    {
        public int ItemSlotsAmount => ItemSlotsConfigs.Length;
        [field: OdinSerialize] public InventoryItemsSlotConfig[] ItemSlotsConfigs { get; private set; }
        [field: SerializeField] public float TimeBeforeStartSetTargetSlotWhenSetTargetSlot { get; private set; } = 0.2f;
        [field: SerializeField] public int StartTargetSlotIndex { get; private set; } = 1;
    }
}