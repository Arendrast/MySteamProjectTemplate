using ProjectName.SharedModule.Runtime.Client.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup.ChangeMouseSensitivity
{
    public class ChangeMouseSensitivitySerializableComponents : MonoBehaviour 
    {
        [field: SerializeField] public TextMeshProUGUI CurrentSensitivityText { get; private set; }
        [field: SerializeField] public Slider Slider { get; private set; }
        [field: SerializeField] public SlicedFilledImage SlicedFilledImage { get; private set; }
    }
}