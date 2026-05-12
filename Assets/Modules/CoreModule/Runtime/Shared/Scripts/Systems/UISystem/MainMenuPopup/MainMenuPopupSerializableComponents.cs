using Modules.SharedModule.Runtime.Client.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.MainMenuPopup
{
    public class MainMenuPopupSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI[] SteamIDTexts { get; private set; }
        [field: SerializeField] public GameObject StartMetricsLevelButton { get; private set; }
        [field: SerializeField] public SelectLevelButtonSerializableComponents[] 
            SelectLevelButtonsSerializableComponents { get; private set; }
        [field: SerializeField] public Button CopySteamIDButton { get; private set; }
        [field: SerializeField] public Button StartAsHostButton { get; private set; }
        [field: SerializeField] public TMP_InputField HostSteamIdInputField { get; private set; }
        [field: SerializeField] public TextMeshProUGUI HostSteamIdBackgroundText { get; private set; }
        [field: SerializeField] public TMP_InputField TargetSafeZoneNumberInputField { get; private set; }
        [field: SerializeField] public TMP_InputField TargetLevelNumberInputField { get; private set; }
        [field: SerializeField] public Toggle IsOperatorToggle { get; private set; }
        [field: SerializeField] public Button StartAsClientButton { get; private set; }
        [field: SerializeField] public Popup Popup { get; private set; }
    }
}