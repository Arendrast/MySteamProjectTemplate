using System.Linq;
using FishNet.Managing.Server;
using MoreLinq;
using ProjectName.LevelModule.Runtime.Shared.Scripts;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.Operator;
using ProjectName.SharedModule.Runtime.Client.Scripts.GameStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using ProjectName.SharedModule.Runtime.Shared.Scripts.GameStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Holders;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using TMPro;
using UnityEngine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.MainMenuPopup
{
    public class MainMenuPopupController
    {
        private int _targetLevelNumber;

        private readonly MainMenuPopupSerializableComponents _popupSerializableComponents;

        public MainMenuPopupController(MainMenuPopupSerializableComponents popupSerializableComponents,
            LevelsConfig levelsConfig, EventBus eventBus, string steamId,
            IsOperatorRepository isOperatorRepository,
            ServerManager serverManager)
        {
            _popupSerializableComponents = popupSerializableComponents;

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
            
            _popupSerializableComponents.SelectLevelButtonsSerializableComponents.ForEach(
                InitializeSelectLevelButton);

            popupSerializableComponents.Popup.TryOpen();

            InitializeSteamPart(steamId);
            ActivateOnlyActiveButton(_popupSerializableComponents.SelectLevelButtonsSerializableComponents.FirstOrDefault());

            return;

            void InitializeSelectLevelButton(SelectLevelButtonSerializableComponents button)
            {
                button.Button.onClick.AddListener(() => ActivateOnlyActiveButton(button));
            }

            void ActivateOnlyActiveButton(SelectLevelButtonSerializableComponents activeButton)
            {
                _popupSerializableComponents.SelectLevelButtonsSerializableComponents.ForEach(button =>
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
                    _popupSerializableComponents.HostSteamIdBackgroundText.text = text);
                _popupSerializableComponents.HostSteamIdBackgroundText.text = inputField.text;

            }

            void TrySendEnterClientStateEvent(bool isHost)
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

                var levelConfig = levelsConfig.LevelsConfigs.SafeGet(_targetLevelNumber - 1);

                if (levelConfig != null)
                    DisableButtonsAndSendEnterClientStateEvent(_targetLevelNumber - 1);

                return;

                void DisableButtonsAndSendEnterClientStateEvent(int targetLevelIndex)
                {
                    DisableButtons();

                    isOperatorRepository.SetIsOperator(popupSerializableComponents.IsOperatorToggle.isOn);

                    var didParseSafeZoneNumber = int.TryParse(
                        popupSerializableComponents.TargetSafeZoneNumberInputField.text,
                        out var targetSafeZoneNumber);

                    eventBus.Fire(new EnterGameStateEvent(GameStateType.MatchGame, new EnterMatchGameStateData(
                        isHost, popupSerializableComponents.HostSteamIdInputField.text,
                        targetLevelIndex, didParseSafeZoneNumber ? targetSafeZoneNumber : 1)));
                }
            }

            void DisableButtons()
            {
                popupSerializableComponents.StartAsClientButton.interactable = false;
                popupSerializableComponents.StartAsHostButton.interactable = false;
            }
        }

        private void InitializeSteamPart(string steamId)
        {
            if (steamId != null)
            {
                _popupSerializableComponents.CopySteamIDButton.onClick.AddListener(UpdateSystemCopyBufferWithSteamId);
                _popupSerializableComponents.SteamIDTexts.ForEach(text => text.text = steamId);
            }

            return;

            void UpdateSystemCopyBufferWithSteamId()
            {
                GUIUtility.systemCopyBuffer = steamId;
            }
        }

        public void TryClosing() => _popupSerializableComponents.Popup.TryClose();
    }
}