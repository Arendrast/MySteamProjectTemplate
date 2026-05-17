using System.Linq;
using FishNet.Managing.Server;
using Modules.LevelModule.Runtime.Shared.Scripts;
using Modules.PlayerModule.Runtime.Shared.Scripts.Operator;
using Modules.SharedModule.Runtime.Client.Scripts.GameStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.GameStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.Holders;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using MoreLinq;
using TMPro;
using UnityEngine;

namespace Modules.AppModule.Runtime.Shared.Scripts.Systems.UISystem.MainMenuPopup
{
    public class MainMenuPopupController
    {
        public MainMenuPopupSerializableComponents PopupSerializableComponents { get; }
        
        private int _targetLevelNumber;

        private readonly IsOperatorRepository _isOperatorRepository;
        private readonly LevelsConfig _levelsConfig;
        private readonly EventBus _eventBus;

        public MainMenuPopupController(MainMenuPopupSerializableComponents popupSerializableComponents,
            LevelsConfig levelsConfig, EventBus eventBus, string steamId,
            IsOperatorRepository isOperatorRepository,
            ServerManager serverManager)
        {
            PopupSerializableComponents = popupSerializableComponents;
            _isOperatorRepository = isOperatorRepository;
            _levelsConfig = levelsConfig;
            _eventBus = eventBus;

            popupSerializableComponents.StartAsHostButton.onClick.AddListener(() =>
                TrySendEnterClientStateEvent(true));
            popupSerializableComponents.StartAsClientButton.onClick.AddListener(() =>
                TrySendEnterClientStateEvent(false));

            AppointValueSubscribeToInputFieldForSaveInPlayerPrefs(popupSerializableComponents.HostSteamIdInputField,
                PlayersPrefsVariablesNamesHolder.HostSteamId);
            // AppointValueSubscribeToInputFieldForSaveInPlayerPrefs(
            //     popupSerialiableComponents.TargetLevelNumberInputField,
            //     PlayersPrefsVariablesNamesHolder.TargetLevelNumber);

#if !UNITY_EDITOR
            _popupSerializableComponents.StartMetricsLevelButton.gameObject.SetActive(false);
#endif

            PopupSerializableComponents.SelectLevelButtonsSerializableComponents.ForEach(
                InitializeSelectLevelButton);

            popupSerializableComponents.Popup.TryOpen();

            InitializeSteamPart(steamId);
            ActivateOnlyActiveButton(PopupSerializableComponents.SelectLevelButtonsSerializableComponents
                .FirstOrDefault());
        }

        public void TrySendEnterClientStateEvent(bool isHost)
        {
            if (!isHost)
            {
                DisableButtonsAndSendEnterClientStateEvent(0);
                return;
            }

            // if (!int.TryParse(popupSerializableComponents.TargetLevelNumberInputField.text,
            //         out var targetLevelNumber))
            // {
            //     return;
            // }

            var levelConfig = _levelsConfig.LevelsConfigs.SafeGet(_targetLevelNumber - 1);

            if (levelConfig != null)
                DisableButtonsAndSendEnterClientStateEvent(_targetLevelNumber - 1);

            return;

            void DisableButtonsAndSendEnterClientStateEvent(int targetLevelIndex)
            {
                DisableButtons();

                _isOperatorRepository.SetIsOperator(PopupSerializableComponents.IsOperatorToggle.isOn);

                var didParseSafeZoneNumber = int.TryParse(
                    PopupSerializableComponents.TargetSafeZoneNumberInputField.text,
                    out var targetSafeZoneNumber);

                _eventBus.Fire(new EnterGameStateEvent(GameStateType.MatchGame, new EnterMatchGameStateData(
                    isHost, PopupSerializableComponents.HostSteamIdInputField.text,
                    targetLevelIndex, didParseSafeZoneNumber ? targetSafeZoneNumber : 1)));
            }
        }

        void InitializeSelectLevelButton(SelectLevelButtonSerializableComponents button)
        {
            button.Button.onClick.AddListener(() => ActivateOnlyActiveButton(button));
        }

        void ActivateOnlyActiveButton(SelectLevelButtonSerializableComponents activeButton)
        {
            PopupSerializableComponents.SelectLevelButtonsSerializableComponents.ForEach(button =>
            {
                button.SelectedImage.gameObject.SetActive(button == activeButton);
                button.UnselectedImage.gameObject.SetActive(button != activeButton);
            });

            _targetLevelNumber = activeButton.LevelNumber;
        }

        void AppointValueSubscribeToInputFieldForSaveInPlayerPrefs(TMP_InputField inputField, string key)
        {
            inputField.text = PlayerPrefs.GetString(key);
            inputField.onValueChanged.AddListener(text => PlayerPrefs.SetString(key, text));
            inputField.onValueChanged.AddListener(text =>
                PopupSerializableComponents.HostSteamIdBackgroundText.text = text);
            PopupSerializableComponents.HostSteamIdBackgroundText.text = inputField.text;
        }

        private void DisableButtons()
        {
            PopupSerializableComponents.StartAsClientButton.interactable = false;
            PopupSerializableComponents.StartAsHostButton.interactable = false;
        }

        private void InitializeSteamPart(string steamId)
        {
            if (steamId != null)
            {
                PopupSerializableComponents.CopySteamIDButton.onClick.AddListener(UpdateSystemCopyBufferWithSteamId);
                PopupSerializableComponents.SteamIDTexts.ForEach(text => text.text = steamId);
            }

            return;

            void UpdateSystemCopyBufferWithSteamId()
            {
                GUIUtility.systemCopyBuffer = steamId;
            }
        }

        public void TryClosing() => PopupSerializableComponents.Popup.TryClose();
    }
}