using System;
using Cysharp.Threading.Tasks;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters;
using ProjectName.ItemModule.Runtime.Shared.Scripts.View;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Configs;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Input;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Volume;
using UnityEngine.Rendering.Universal;

namespace ProjectName.HudModule.Runtime.Scripts.HudPopup
{
    public class HudPopupFactory : IMatchSharedFactory, IDisposable
    {
        private readonly HashedAssetProvider _hashedAssetProvider;
        private readonly ConfigsProviderService _configsProviderService;
        private readonly EventBus _eventBus;
        private readonly IInputProvider _inputProvider;
        private readonly TimeScaleRepository _timeScaleProvider;
        private readonly DynamicGlobalVolumeFactory _dynamicVolumeFactory;

        private const string HudPopupAssetId = "HudPopup";

        public HudPopupFactory(HashedAssetProvider hashedAssetProvider,
            ConfigsProviderService configsProviderService,
            EventBus eventBus, IInputProvider inputProvider, TimeScaleRepository timeScaleProvider,
            DynamicGlobalVolumeFactory dynamicVolumeFactory)
        {
            _hashedAssetProvider = hashedAssetProvider;
            _configsProviderService = configsProviderService;
            _eventBus = eventBus;
            _inputProvider = inputProvider;
            _timeScaleProvider = timeScaleProvider;
            _dynamicVolumeFactory = dynamicVolumeFactory;
        }

        public void Dispose()
        {
            DisposeAsync().Forget();
        }

        public async UniTask DisposeAsync()
        {
            await _hashedAssetProvider.DisposeAsync();
        }

        public async UniTask<HudPopupController> GetCreatedHudPopupControllerAsync(
            OwnerPlayerComponents ownerPlayerComponents,
            NetworkCountersSynchronizerBehaviour countersSynchronizerBehaviour)
        {
            return await _hashedAssetProvider.GetControllerAsync<HudPopupController, HudPopupSerializableComponents>(
                HudPopupAssetId,
                async instance =>
                {
                    _dynamicVolumeFactory.Volume.MainVolume.profile.TryGet<Vignette>(out var mainVignette);

                    _hashedAssetProvider.RegisterAndGetSingleByType(new HudPopupController(instance,
                        await _configsProviderService.GetConfigAsync<ItemsViewConfig>(),
                        _eventBus,
                        _inputProvider,
                        _timeScaleProvider, mainVignette,
                        ownerPlayerComponents, countersSynchronizerBehaviour));
                });
        }
    }
}

// Arendrast