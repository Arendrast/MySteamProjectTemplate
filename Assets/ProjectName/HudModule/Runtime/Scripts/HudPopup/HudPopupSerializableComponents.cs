using ProjectName.HudModule.Runtime.Scripts.CheatMenu;
using ProjectName.HudModule.Runtime.Scripts.GameHint;
using ProjectName.HudModule.Runtime.Scripts.LowHealPoints;
using ProjectName.InventoryModule.Runtime.Shared.Scripts.UI;
using ProjectName.SharedModule.Runtime.Client.Scripts.UI;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using ProjectName.SharedModule.Runtime.Shared.Scripts.UI;
using TMPro;
using UnityEngine;

namespace ProjectName.HudModule.Runtime.Scripts.HudPopup
{
    public class HudPopupSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI InteractText { get; private set; }
        [field: SerializeField] public Popup Popup { get; private set; }
        [field: SerializeField] public InventoryItemsWindowSerializableComponents InventoryItemsWindowSerializableComponents { get; private set;}
        [field: SerializeField] public TextMeshProUGUI SpeedTText { get; private set; }
        [field: SerializeField] public BarSerializableComponents ReviveBarSerializableComponents { get; private set; }
        [field: SerializeField] public MonoBehaviourObserver MonoBehaviourObserver { get; private set; }
        [field: SerializeField] public CheatMenuPopupSerializableComponents CheatMenuPopupSerializableComponents { get; private set; }
        [field: SerializeField] public LowHealPointsPopupSerializableComponents LowHealPointsPopupSerializableComponents { get; private set; }
        [field: SerializeField] public GameHintWindowSerializableComponents GameHintsWindowSerializableComponents { get; private set; }
    }
}
