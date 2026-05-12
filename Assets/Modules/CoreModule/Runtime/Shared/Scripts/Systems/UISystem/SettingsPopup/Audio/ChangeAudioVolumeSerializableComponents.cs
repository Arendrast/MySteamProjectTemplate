using Modules.SharedModule.Runtime.Client.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup.Audio
{
    public class ChangeAudioVolumeSerializableComponents : MonoBehaviour 
    {
        [field: SerializeField] public Slider Slider { get; private set; }
        [field: SerializeField] public SlicedFilledImage SlicedFilledImage { get; private set; }
        [field: SerializeField] public ChangeAudioVolumeConfig Config { get; private set; }
    }
}