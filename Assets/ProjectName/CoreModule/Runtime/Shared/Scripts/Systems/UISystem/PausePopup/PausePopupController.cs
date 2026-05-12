using ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup;
using ProjectName.SharedModule.Runtime.Client.Scripts.GameStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Input;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.PausePopup
{
    public class PausePopupController
    {
        private SettingsPopupController _settingsPopupController;

        private bool _isPaused, _wasCursorEnabled;
        private readonly PausePopupSerializableComponents _pausePopupSerializableComponents;
        private readonly TimeScaleRepository _timeScaleRepository;
        private readonly IInputProvider _inputProvider;

        public PausePopupController(IInputProvider inputProvider, TimeScaleRepository timeScaleRepository,
            PausePopupSerializableComponents pausePopupSerializableComponents, EventBus eventBus,
            SettingsPopupFactory settingsPopupFactory)
        {
            _inputProvider = inputProvider;
            _timeScaleRepository = timeScaleRepository;
            _pausePopupSerializableComponents = pausePopupSerializableComponents;

            pausePopupSerializableComponents.GetOrAddComponent<MonoBehaviourObserver>().Updated += TrySetIsPaused;

            pausePopupSerializableComponents.ExitToMenuButton.onClick.AddListener(() =>
                eventBus.Fire(new EnterGameStateEvent(GameStateType.HardReset)));

            pausePopupSerializableComponents.SettingsButton.onClick.AddListener(TryOpenSettingsPopup);

            pausePopupSerializableComponents.ExitButton.onClick.AddListener(Application.Quit);

            pausePopupSerializableComponents.SecretButton.onClick.AddListener(() =>
                Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ"));

            AppointSettingsPopupControllerAsync();

            return;

            async void AppointSettingsPopupControllerAsync()
            {
                _settingsPopupController = await settingsPopupFactory.GetSettingsPopupControllerAsync();
            }

            void TryOpenSettingsPopup()
            {
                _settingsPopupController?.TryOpen();
            }
        }

        public void TryClosePopup()
        {
            _pausePopupSerializableComponents.Popup.TryClose();
            _settingsPopupController?.TryClose();

            if (_isPaused)
                SetIsPaused();
        }

        private void SetIsPaused()
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                _wasCursorEnabled = CursorSwitchTools.IsCursorEnabled;
            }

            CursorSwitchTools.TrySwitchCursor(_isPaused || _wasCursorEnabled);
            _timeScaleRepository.SetTimeScale(_isPaused ? 0 : 1);
            _pausePopupSerializableComponents.Popup.TrySetOpenState(_isPaused);

            if (!_isPaused)
                _settingsPopupController?.TryClose();
        }

        private void TrySetIsPaused()
        {
            if (!_inputProvider.IsActionTriggered(InputActionType.Pause))
                return;

            SetIsPaused();
        }
    }
}