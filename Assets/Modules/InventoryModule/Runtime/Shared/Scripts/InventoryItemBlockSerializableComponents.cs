using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.InventoryModule.Runtime.Shared.Scripts
{
    public class InventoryItemBlockSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public Image ItemImage { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ItemsCountText { get; private set; }
    }
}