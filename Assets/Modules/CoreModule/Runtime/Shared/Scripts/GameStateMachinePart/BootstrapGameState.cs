using Cysharp.Threading.Tasks;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Client.Scripts.GameStateMachine;
using Modules.SharedModule.Runtime.Client.Scripts.UI.Cursor;
using Modules.SharedModule.Runtime.Shared.Scripts.CameraPart;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.GameStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart
{
    public class BootstrapGameState : IGameState
    {
        private readonly CameraFactory _cameraFactory;
        private readonly EventBus _eventBus;
        private readonly DebugEnableProviderService _debugEnableProviderService;
        private readonly SteamEditorConfig _steamEditorConfig;
        private readonly CursorConfig _cursorConfig;

        public BootstrapGameState(CameraFactory cameraFactory, EventBus eventBus,
            DebugEnableProviderService debugEnableProviderService, SteamEditorConfig steamEditorConfig, CursorConfig cursorConfig)
        {
            _cameraFactory = cameraFactory;
            _eventBus = eventBus;
            _debugEnableProviderService = debugEnableProviderService;
            _steamEditorConfig = steamEditorConfig;
            _cursorConfig = cursorConfig;
        }

        public async UniTask EnterAsync(IGameStateEnterData data)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = -1;
            Cursor.SetCursor(_cursorConfig.Texture, _cursorConfig.HotSpot, CursorMode.Auto);

            if (_steamEditorConfig.ShouldUseSteam)
            {
                SteamTools.Game.Initialize();

                await UniTask.WaitWhile(() => !Interface.IsReady);

                Interface.IsDebugging = true;
            }

            Popcron.Gizmos.FrustumCulling = false;
            Popcron.Gizmos.Enabled = _debugEnableProviderService.Enable;
            _debugEnableProviderService.EnableChanged += () => { Popcron.Gizmos.Enabled = _debugEnableProviderService.Enable; };

            await _cameraFactory.GetCreatedMainCameraAsync();
            _eventBus.Fire(new EnterGameStateEvent(GameStateType.MainMenu));
        }

        public UniTask ExitAsync() => UniTask.CompletedTask;
    }
}