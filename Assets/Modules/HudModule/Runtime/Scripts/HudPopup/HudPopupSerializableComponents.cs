using Modules.HudModule.Runtime.Scripts.CheatMenu;
using Modules.HudModule.Runtime.Scripts.GameHint;
using Modules.HudModule.Runtime.Scripts.LowHealPoints;
using Modules.InventoryModule.Runtime.Shared.Scripts.UI;
using Modules.SharedModule.Runtime.Client.Scripts.UI;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.UI;
using TMPro;
using UnityEngine;

namespace Modules.HudModule.Runtime.Scripts.HudPopup
{
    public class HudPopupSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI InteractText { get; private set; }
        [field: SerializeField] public Popup Popup { get; private set; }
        [field: SerializeField] public InventoryItemsWindowSerializableComponents InventoryItemsWindowSerializableComponents { get; private set;}
        [field: SerializeField] public TextMeshProUGUI SpeedTText { get; private set; }
        [field: SerializeField] public BarSerializableComponents ReviveBarSerializableComponents { get; private set; }
        [field: SerializeField] public CheatMenuPopupSerializableComponents CheatMenuPopupSerializableComponents { get; private set; }
        [field: SerializeField] public LowHealPointsPopupSerializableComponents LowHealPointsPopupSerializableComponents { get; private set; }
        [field: SerializeField] public GameHintWindowSerializableComponents GameHintsWindowSerializableComponents { get; private set; }
    }
}
