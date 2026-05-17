using System.Collections.Generic;
using Modules.AppModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup.Audio;
using Modules.AppModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup.ChangeMouseSensitivity;
using Modules.SharedModule.Runtime.Client.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.AppModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup
{
    public class SettingsPopupSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public SettingsPopupConfig PopupConfig { get; private set; }   
        [field: SerializeField] public Popup Popup { get; private set; }
        [field: SerializeField] public Toggle DisableHUDToggle { get; private set; }
        [field: SerializeField] public List<ChangeAudioVolumeSerializableComponents> VolumeViews { get; private set; }
        [field: SerializeField] public ChangeMouseSensitivitySerializableComponents ChangeMouseSensitivitySerializableComponents { get; private set; }
    }
}