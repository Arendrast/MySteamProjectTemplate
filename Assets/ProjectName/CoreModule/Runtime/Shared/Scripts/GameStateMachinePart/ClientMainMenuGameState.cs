using Cysharp.Threading.Tasks;
using ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.LoadingPopup;
using ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.MainMenuPopup;
using ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.PausePopup;
using ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup;
using ProjectName.SharedModule.Runtime.Shared.Scripts.GameStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Holders;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine.SceneManagement;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart
{
    public class ClientMainMenuGameState : IGameState
    {
        private readonly MainMenuPopupFactory _mainMenuPopupFactory;
        private readonly SettingsPopupFactory _settingsPopupFactory;
        private readonly PausePopupFactory _pausePopupFactory;
        private readonly LoadingPopupFactory _loadingPopupFactory;

        public ClientMainMenuGameState(MainMenuPopupFactory mainMenuPopupFactory, PausePopupFactory pausePopupFactory,
            LoadingPopupFactory loadingPopupFactory)
        {
            _mainMenuPopupFactory = mainMenuPopupFactory;
            _pausePopupFactory = pausePopupFactory;
            _loadingPopupFactory = loadingPopupFactory;
        }

        public async UniTask EnterAsync(IGameStateEnterData data)
        {
            SceneManager.LoadScene(ScenesNamesHolder.MainMenu);
            var loadingGamePopupController = await _mainMenuPopupFactory.GetMainMenuPopupControllerAsync();
            var pausePopupController = await _pausePopupFactory.GetPausePopupControllerAsync();
            CursorSwitchTools.TryEnableCursor();
        }

        public async UniTask ExitAsync()
        {
            await _loadingPopupFactory.GetLoadingPopupControllerAsync();
            await _mainMenuPopupFactory.DisposeAsync();
        }
    }
}