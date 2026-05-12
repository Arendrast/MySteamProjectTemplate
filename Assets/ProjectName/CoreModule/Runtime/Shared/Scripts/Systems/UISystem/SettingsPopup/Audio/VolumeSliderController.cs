using ProjectName.SharedModule.Runtime.Client.Scripts.UI;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Holders;
using UnityEngine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup.Audio
{
    public class VolumeSliderController
    {
        private string NameVariableSliderInPlayerPref => PlayersPrefsVariablesNamesHolder.GetAudioVolume(_config.AudioType.ToString());
        
        private readonly ChangeAudioVolumeConfig _config;

        private float _currentVolume = 1;

        public VolumeSliderController(ChangeAudioVolumeSerializableComponents serializableComponents,
            ChangeAudioVolumeConfig config)
        {
            _config = config;

            if (!PlayerPrefs.HasKey(NameVariableSliderInPlayerPref))
                SaveValueSliderInPlayerPref();

            serializableComponents.SlicedFilledImage.SubscribeToSetFillAmountBySliderValue(serializableComponents.Slider);
            serializableComponents.Slider.maxValue = 1f;
            serializableComponents.Slider.minValue = 0f;
            serializableComponents.Slider.value = PlayerPrefs.GetFloat(NameVariableSliderInPlayerPref);
            serializableComponents.Slider.onValueChanged.AddListener(SetVolume);

            SetVolume(serializableComponents.Slider.value);
        }

        private void SetVolume(float value)
        {
            _currentVolume = value;
            #if WWISE
            AkSoundEngine.SetRTPCValue(_config.AudioType.ToString(), value * _config.VolumeMultiplier);
            #endif
            SaveValueSliderInPlayerPref();
        }

        private void SaveValueSliderInPlayerPref() => PlayerPrefs.SetFloat(NameVariableSliderInPlayerPref, _currentVolume);
    }
}