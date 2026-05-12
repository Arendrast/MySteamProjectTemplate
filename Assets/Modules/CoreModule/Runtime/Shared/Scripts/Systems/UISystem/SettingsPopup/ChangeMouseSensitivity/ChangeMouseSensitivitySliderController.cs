using System.Globalization;
using Modules.SharedModule.Runtime.Client.Scripts.UI;
using Modules.SharedModule.Runtime.Shared.Scripts.Holders;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup.ChangeMouseSensitivity
{
    public class ChangeMouseSensitivitySliderController
    {
        private readonly MouseSensitivityRepository _mouseSensitivityRepository;
        private readonly ChangeMouseSensitivitySerializableComponents _serializableComponents;

        public ChangeMouseSensitivitySliderController(MouseSensitivityRepository mouseSensitivityRepository,
            ChangeMouseSensitivitySerializableComponents serializableComponents)
        {
            _mouseSensitivityRepository = mouseSensitivityRepository;
            _serializableComponents = serializableComponents;

            if (!PlayerPrefs.HasKey(PlayersPrefsVariablesNamesHolder.MouseSensitivity))
                SaveValueSliderInPlayerPref();
            
            serializableComponents.SlicedFilledImage.SubscribeToSetFillAmountBySliderValue(serializableComponents.Slider);
            serializableComponents.Slider.value = PlayerPrefs.GetFloat(PlayersPrefsVariablesNamesHolder.MouseSensitivity);
            serializableComponents.Slider.onValueChanged.AddListener(SetSensitivity);
            
            SetSensitivity(serializableComponents.Slider.value);
        }

        private void SetSensitivity(float value)
        {
            _mouseSensitivityRepository.SetCurrentSensitivity(value);
            _serializableComponents.CurrentSensitivityText.text = value.ToString("f1", CultureInfo.InvariantCulture);
            
            SaveValueSliderInPlayerPref();
        }

        private void SaveValueSliderInPlayerPref() => PlayerPrefs.SetFloat(
            PlayersPrefsVariablesNamesHolder.MouseSensitivity, _mouseSensitivityRepository.CurrentSensitivity);
    }
}