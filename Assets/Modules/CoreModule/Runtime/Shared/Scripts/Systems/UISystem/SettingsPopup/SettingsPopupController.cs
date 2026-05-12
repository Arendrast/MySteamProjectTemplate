using Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup.Audio;
using Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup.ChangeMouseSensitivity;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem.Events;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup
{
    public class SettingsPopupController
    {
        private readonly SettingsPopupSerializableComponents _serializableComponents;
        private readonly EventBus _eventBus;

        public SettingsPopupController(SettingsPopupSerializableComponents serializableComponents,
            MouseSensitivityRepository mouseSensitivityRepository,
            EventBus eventBus)
        {
            _serializableComponents = serializableComponents;
            _eventBus = eventBus;

            SubscribeButtons();
            new ChangeMouseSensitivitySliderController(mouseSensitivityRepository,
                serializableComponents.ChangeMouseSensitivitySerializableComponents);
            CreateVolumeSliderControllers();
            
            serializableComponents.DisableHUDToggle.onValueChanged.AddListener(SetCanShowHud);
        }

        private void SetCanShowHud(bool disable)
        {
            _eventBus.Fire(new SetOpenStateHudPopupEvent(!disable));
        }

        public void TryOpen()
        {
            _serializableComponents.Popup.TryOpen();
        }

        public void TryClose()
        {
            _serializableComponents.Popup.TryClose();
        }

        private void CreateVolumeSliderControllers()
        {
            foreach (var changingAudioView in _serializableComponents.VolumeViews)
            {
                if (changingAudioView.Config == null)
                    continue;

                new VolumeSliderController(changingAudioView, changingAudioView.Config);
            }
        }

        private void SubscribeButtons()
        {
        }
    }
}