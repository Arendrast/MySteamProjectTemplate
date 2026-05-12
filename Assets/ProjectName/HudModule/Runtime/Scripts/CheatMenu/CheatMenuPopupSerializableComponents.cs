using ProjectName.SharedModule.Runtime.Client.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectName.HudModule.Runtime.Scripts.CheatMenu
{
    public class CheatMenuPopupSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public Popup Popup { get; private set; }
        [field: SerializeField] public Toggle IsImmortalToggle { get; private set; }
        [field: SerializeField] public Toggle IsUnpushableToggle { get; private set; }
    }
}