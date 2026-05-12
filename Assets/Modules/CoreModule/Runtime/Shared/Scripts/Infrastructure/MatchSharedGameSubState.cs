using System;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Client;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters;
using Modules.HudModule.Runtime.Scripts.HudPopup;
using Modules.NetworkModule.Runtime.Shared.Scripts.NetworkTimer;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.Operator;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using Modules.SharedModule.Runtime.Shared.Scripts.CameraPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using Modules.SharedModule.Runtime.Shared.Scripts.Volume;
using UnityEngine.Rendering.Universal;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public class MatchSharedGameSubState : IMatchSharedService, IDisposable
    {
        private readonly OperatorFactory _operatorFactory;
        private readonly DynamicGlobalVolumeFactory _dynamicGlobalVolumeFactory;
        private readonly CameraFactory _cameraFactory;
        private readonly IPlayerSpawnerPositionsProvider _playerSpawnerPositionsProvider;
        private readonly NetworkCountersSynchronizerBehaviourRepository _networkCountersSynchronizerBehaviourRepository;
        private readonly InputActions _inputActions;
        private readonly NetworkTimerService _networkTimerService;
        private readonly ClientManager _clientManager;
        private readonly OwnerPlayerFactory _ownerPlayerFactory;
        private readonly HudPopupFactory _hudPopupFactory;

        public MatchSharedGameSubState(OperatorFactory operatorFactory,
            DynamicGlobalVolumeFactory dynamicGlobalVolumeFactory, CameraFactory cameraFactory,
            IPlayerSpawnerPositionsProvider playerSpawnerPositionsProvider,
            NetworkCountersSynchronizerBehaviourRepository networkCountersSynchronizerBehaviourRepository,
            InputActions inputActions, NetworkTimerService networkTimerService, ClientManager clientManager,
            OwnerPlayerFactory ownerPlayerFactory, HudPopupFactory hudPopupFactory)
        {
            _operatorFactory = operatorFactory;
            _dynamicGlobalVolumeFactory = dynamicGlobalVolumeFactory;
            _cameraFactory = cameraFactory;
            _playerSpawnerPositionsProvider = playerSpawnerPositionsProvider;
            _networkCountersSynchronizerBehaviourRepository = networkCountersSynchronizerBehaviourRepository;
            _inputActions = inputActions;
            _networkTimerService = networkTimerService;
            _clientManager = clientManager;
            _ownerPlayerFactory = ownerPlayerFactory;
            _hudPopupFactory = hudPopupFactory;
        }

        public void Dispose()
        {
            Exit();
        }

        public async UniTask EnterAsync(bool isOperator, ISharedSubscribingMediator subscribingMediator)
        {
            await _dynamicGlobalVolumeFactory.GetCreatedVolumeAsync();

            if (_dynamicGlobalVolumeFactory.Volume.BurningVolume.profile.TryGet<Vignette>(out var burningVignette))
                burningVignette.active = false;

            if (_dynamicGlobalVolumeFactory.Volume.MainVolume.profile.TryGet<Vignette>(out var mainVignette))
                mainVignette.active = false;

            var camera = await _cameraFactory.GetCreatedMainCameraAsync();

            if (isOperator)
            {
                await ConfigureOperatorAsync(camera);
            }
            else
            {
                var spawnPoint = (_playerSpawnerPositionsProvider
                                      .SpawnersPositions.SafeGet(
                                          4 * (_networkCountersSynchronizerBehaviourRepository.Behaviour.Counters[
                                              CounterType.TargetSafeZone]) +
                                          _clientManager.Clients.Count - 1)
                                  ?? _playerSpawnerPositionsProvider
                                      .SpawnersPositions.SafeGet(_clientManager.Clients.Count - 1)).position;

                var ownerPlayerComponents =
                    await _ownerPlayerFactory.GetCreatedOwnerPlayerComponentsAsync(
                        _clientManager.GetOwnerConnection(),
                        position: spawnPoint);

                var hudPopupController = await _hudPopupFactory.GetCreatedHudPopupControllerAsync(
                    ownerPlayerComponents, _networkCountersSynchronizerBehaviourRepository.Behaviour);
            }

            subscribingMediator.SubscribeAfterInitialize();

            CursorSwitchTools.TryDisableCursor();
            _inputActions.Enable();
            _networkTimerService.Enable();

            camera.FPSCameraController.SetIsEnabledRotateCameraByLookInput(true);
        }

        private async UniTask ConfigureOperatorAsync(CameraComponents camera)
        {
            await _operatorFactory.GetCreatedOperatorMovementController();
            camera.FPSCameraController.SetHorizontalAngleConstraints(0, 0);
            camera.FPSCameraController.SetShouldRotateByLookInputX(true);
        }

        private void Exit()
        {
        }
    }
}