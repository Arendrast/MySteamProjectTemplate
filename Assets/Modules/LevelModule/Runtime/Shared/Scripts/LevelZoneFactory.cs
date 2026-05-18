using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using FishNet.Object;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactor;
using Modules.EntityModule.Runtime.Shared.Scripts;
using Modules.EntityModule.Runtime.Shared.Scripts.Destroyable.Logic;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects;
using Modules.EntityModule.Runtime.Shared.Scripts.Push;
using Modules.InteractableModule.Runtime.Shared.Scripts.Interactables;
using Modules.SharedModule.Runtime.Shared.Scripts;
using Modules.SharedModule.Runtime.Shared.Scripts.Loading;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Modules.LevelModule.Runtime.Shared.Scripts
{
    public class LevelZoneFactory : IMatchSharedFactory, IDisposable, ITargetZoneNumberProvider
    {
        public int TargetZoneNumber { get; private set; }

        private LevelConfig _levelConfig;

        private readonly IAssetLoader _assetLoader;
        private readonly InteractablesFactory _interactablesFactory;
        private readonly ServerManager _serverManager;
        private readonly DestroyablesFactory _destroyablesFactory;
        private readonly RigidbodyPushablesFactory _rigidbodyPushablesFactory;
        private readonly ExplodersFactory _explodersFactory;
        private readonly EffectApplierFactory _effectApplierFactory;
        private readonly LevelZoneRepository _levelZoneRepository;
        private readonly ActionTriggerReactorsFactory _actionTriggerReactorsFactory;
        private readonly ForbiddenZoneFactory _forbiddenZoneFactory;

        public LevelZoneFactory(IAssetLoader assetLoader,
            InteractablesFactory interactablesFactory,
            ServerManager serverManager,
            RigidbodyPushablesFactory rigidbodyPushablesFactory,
            ExplodersFactory explodersFactory,
            DestroyablesFactory destroyablesFactory,
            EffectApplierFactory effectApplierFactory,
            LevelZoneRepository levelZoneRepository,
            ActionTriggerReactorsFactory actionTriggerReactorsFactory,
            LevelZoneFactoryRepository levelZoneFactoryRepository,
            ForbiddenZoneFactory forbiddenZoneFactory)
        {
            _assetLoader = assetLoader;
            _interactablesFactory = interactablesFactory;
            _serverManager = serverManager;
            _rigidbodyPushablesFactory = rigidbodyPushablesFactory;
            _explodersFactory = explodersFactory;
            _destroyablesFactory = destroyablesFactory;
            _effectApplierFactory = effectApplierFactory;
            _levelZoneRepository = levelZoneRepository;
            _actionTriggerReactorsFactory = actionTriggerReactorsFactory;
            _forbiddenZoneFactory = forbiddenZoneFactory;

            levelZoneFactoryRepository.LevelZoneFactory = this;
        }

        public void Dispose()
        {
            _levelConfig = null;
        }

        public async UniTask TryInitializeClientLevelZoneAsync(
            LevelZoneSerializableComponents levelZoneSerializableComponents)
        {
            TrySetTargetZone(levelZoneSerializableComponents);

            CreateAllExploders(levelZoneSerializableComponents);
            InitializeAllDestroyables(levelZoneSerializableComponents);
            InitializeAllActionTriggerReactors(levelZoneSerializableComponents);
            InitializeForbiddenZones(levelZoneSerializableComponents);
            await CreateOnlyNotSyncableInteractablesAsync();

            ConfigureLightingAsync(levelZoneSerializableComponents);

            return;

            async UniTask CreateOnlyNotSyncableInteractablesAsync()
            {
                foreach (var interactableSerializableComponents in levelZoneSerializableComponents.
                             ChildrenSerializableComponentsContainer
                             .GetContainedChildren<InteractableSerializableComponents>())
                {
                    await _interactablesFactory.GetCreatedInteractableAsync(interactableSerializableComponents,
                        flag: CreateInteractableFlag.OnlyNotSyncable);
                }
            }
        }

        public async UniTask<LevelZoneSerializableComponents> InitializeStartLevelZones(
            LevelConfig levelConfig)
        {
            if (_levelZoneRepository.TargetLevelZoneSerializableComponents != null)
                return _levelZoneRepository.TargetLevelZoneSerializableComponents;

            LevelZoneSerializableComponents persistentObjectsLevelZoneSerializableComponents = null;
            LevelZoneSerializableComponents targetLevelZoneSerializableComponents = null;

            _levelConfig = levelConfig;

#if UNITY_EDITOR
            var zones =
                Object.FindObjectsOfType<LevelZoneSerializableComponents>(true);

            persistentObjectsLevelZoneSerializableComponents =
                await GetInitializedServerLevelZoneAsync(levelConfig,
                    zones.FirstOrDefault(zone => zone.IsPersistent));

            targetLevelZoneSerializableComponents =
                await GetInitializedServerLevelZoneAsync(levelConfig,
                    zones.FirstOrDefault(zone => !zone.IsPersistent));
#endif

            persistentObjectsLevelZoneSerializableComponents ??=
                await GetInitializedServerLevelZoneAsync(1, true);

            targetLevelZoneSerializableComponents ??=
                await GetInitializedServerLevelZoneAsync(levelConfig.StartLevelIndex + 1);

            _levelZoneRepository.SetPersistentObjectsZoneEnvironmentSerializableComponents(
                persistentObjectsLevelZoneSerializableComponents);
            _levelZoneRepository.SetTargetZoneEnvironmentSerializableComponents(
                targetLevelZoneSerializableComponents);

            return targetLevelZoneSerializableComponents;
        }

        public async UniTask<LevelZoneSerializableComponents> GetInitializedServerLevelZoneAsync(
            int zoneNumber,
            bool isPersistentZone = false)
        {
            TargetZoneNumber = zoneNumber;

            return await GetInitializedServerLevelZoneAsync(_levelConfig, await AssetProvider
                .InstantiateAsync<LevelZoneSerializableComponents>(
                    _levelConfig.LevelName + (isPersistentZone ? "PersistentZone" : "Zone" + zoneNumber),
                    _assetLoader));
        }

        private async UniTask<LevelZoneSerializableComponents> GetInitializedServerLevelZoneAsync(
            LevelConfig levelConfig, LevelZoneSerializableComponents levelZoneSerializableComponents)
        {
            if (levelZoneSerializableComponents == null)
                return null;

            TrySetTargetZone(levelZoneSerializableComponents);

            _serverManager.TryCustomSpawn(levelZoneSerializableComponents.gameObject);

            InitializeForbiddenZones(levelZoneSerializableComponents);
            CreateAllEffectAppliers();
            InitializeAllDestroyables(levelZoneSerializableComponents);
            await CreateAllInteractablesAsync();
            CreateAllExplosionHandlers(levelZoneSerializableComponents);
            CreateAllExploders(levelZoneSerializableComponents);
            InitializeAllActionTriggerReactors(levelZoneSerializableComponents);

            ConfigureLightingAsync(levelZoneSerializableComponents);

            return levelZoneSerializableComponents;

            void CreateAllEffectAppliers()
            {
                foreach (var effectApplierSerializableComponents in levelZoneSerializableComponents
                             .ChildrenSerializableComponentsContainer
                             .GetContainedChildren<EffectApplierSerializableComponents>())
                {
                    _effectApplierFactory.GetCreatedEffectApplierController(effectApplierSerializableComponents,
                        effectApplierSerializableComponents.EffectType,
                        effectApplierSerializableComponents.GetComponent<NetworkObject>().ObjectId);
                }
            }

            void CreateAllExplosionHandlers(LevelZoneSerializableComponents levelZoneSerializableComponents)
            {
                foreach (var explodable in levelZoneSerializableComponents
                             .ChildrenSerializableComponentsContainer
                             .GetContainedChildren<ExplodableSerializableComponents>())
                {
                    _rigidbodyPushablesFactory.TryCreateRigidbodyPushHandler(explodable);
                }
            }

            async UniTask CreateAllInteractablesAsync()
            {
                foreach (var interactableSerializableComponents in levelZoneSerializableComponents
                             .ChildrenSerializableComponentsContainer
                             .GetContainedChildren<InteractableSerializableComponents>())
                {
                    await _interactablesFactory.GetCreatedInteractableAsync(interactableSerializableComponents);
                }
            }
        }

        private async void ConfigureLightingAsync(LevelZoneSerializableComponents levelZoneSerializableComponents)
        {
            if (levelZoneSerializableComponents.IsPersistent || !levelZoneSerializableComponents.UseLightingConfig)
            {
                return;
            }

            var lightingConfig = levelZoneSerializableComponents.LightingConfig;

            await LoadAndAppointSkyboxAsync();

            RenderSettings.subtractiveShadowColor = lightingConfig.RealtimeShadowColor;

            RenderSettings.ambientMode = lightingConfig.EnvironmentLightingConfig.Source;

            switch (lightingConfig.EnvironmentLightingConfig.Source)
            {
                case AmbientMode.Skybox:
                    RenderSettings.ambientIntensity = lightingConfig.EnvironmentLightingConfig.AmbientIntensity;
                    break;
                case AmbientMode.Trilight:
                    RenderSettings.ambientSkyColor = lightingConfig.EnvironmentLightingConfig.AmbientSkyColor;
                    RenderSettings.ambientEquatorColor = lightingConfig.EnvironmentLightingConfig.EquatorColor;
                    RenderSettings.ambientGroundColor = lightingConfig.EnvironmentLightingConfig.GroundColor;
                    break;
                case AmbientMode.Flat:
                    RenderSettings.ambientLight = lightingConfig.EnvironmentLightingConfig.AmbientColor;
                    break;
            }

            RenderSettings.defaultReflectionMode = lightingConfig.EnvironmentReflectionsConfig.DefaultMode;
            RenderSettings.defaultReflectionResolution = lightingConfig.EnvironmentReflectionsConfig.DefaultResolution;
            RenderSettings.reflectionIntensity = lightingConfig.EnvironmentReflectionsConfig.IntensityMultiplier;
            RenderSettings.reflectionBounces = lightingConfig.EnvironmentReflectionsConfig.Bounces;

            RenderSettings.fog = lightingConfig.FogConfig.Enable;
            RenderSettings.fogColor = lightingConfig.FogConfig.Color;
            RenderSettings.fogMode = lightingConfig.FogConfig.Mode;
            RenderSettings.fogStartDistance = lightingConfig.FogConfig.Start;
            RenderSettings.fogEndDistance = lightingConfig.FogConfig.End;

            RenderSettings.haloStrength = lightingConfig.HaloConfig.Strength;
            RenderSettings.flareFadeSpeed = lightingConfig.FlareConfig.FadeSpeed;
            RenderSettings.flareStrength = lightingConfig.FlareConfig.Strength;

            DynamicGI.UpdateEnvironment();

            return;

            async UniTask LoadAndAppointSkyboxAsync()
            {
                var releaseActionContainer = new DataContainer<Action>();

                var skyBox = await AssetProvider.LoadAsync<Material>(lightingConfig.SkyBoxMaterialReference,
                    _assetLoader, releaseActionContainer);

                if (skyBox == null && lightingConfig.CanSkyBoxBeNull || skyBox != null)
                {
                    RenderSettings.skybox = skyBox;
                }

                levelZoneSerializableComponents.GetOrAddComponent<DestroyObserver>().Destroyed +=
                    () => releaseActionContainer.Data?.Invoke();
            }
        }

        private void CreateAllExploders(LevelZoneSerializableComponents levelZoneSerializableComponents)
        {
            foreach (var exploder in levelZoneSerializableComponents
                         .ChildrenSerializableComponentsContainer
                         .GetContainedChildren<ExploderSerializableComponents>())
            {
                _explodersFactory.TryCreateExploder(exploder);
            }
        }

        private void TrySetTargetZone(LevelZoneSerializableComponents levelZoneSerializableComponents)
        {
            if (levelZoneSerializableComponents.IsPersistent) return;
            _levelZoneRepository.SetTargetZoneEnvironmentSerializableComponents(
                levelZoneSerializableComponents);
        }

        private void InitializeAllActionTriggerReactors(
            LevelZoneSerializableComponents levelZoneSerializableComponents)
        {
            foreach (var reactor in levelZoneSerializableComponents
                         .ChildrenSerializableComponentsContainer
                         .GetContainedChildren<ActionTriggerReactorSerializableComponents>())
            {
                _actionTriggerReactorsFactory.TryInitializeReactorAsync(reactor).Forget();
            }
        }


        private void InitializeAllDestroyables(
            LevelZoneSerializableComponents levelZoneSerializableComponents)
        {
            foreach (var destroyable in levelZoneSerializableComponents
                         .ChildrenSerializableComponentsContainer
                         .GetContainedChildren<DestroyableSerializableComponents>())
            {
                _destroyablesFactory.InitializeDestroyable(destroyable, out _);
            }
        }

        private void InitializeForbiddenZones(LevelZoneSerializableComponents levelZoneSerializableComponents)
        {
            foreach (var forbiddenZone in levelZoneSerializableComponents
                         .ChildrenSerializableComponentsContainer
                         .GetContainedChildren<ForbiddenZoneSerializableComponents>())
            {
                _forbiddenZoneFactory.InitializeForbiddenZone(forbiddenZone);
            }
        }
    }
}