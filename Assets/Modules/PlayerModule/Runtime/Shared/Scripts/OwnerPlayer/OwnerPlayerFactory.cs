using System;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Push;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network;
using Modules.InventoryModule.Runtime.Shared.Scripts;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.DependencyInjection;
using Modules.PlayerModule.Runtime.Shared.Scripts.Operator;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.StateMachinePart;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.HandsStates;
using Modules.SharedModule.Runtime.Shared.Scripts.CameraPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.Loading;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using VContainer;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer
{
    public class OwnerPlayerFactory : IMatchSharedFactory,
        IOwnerInteractionObserverProvider,
        IOwnerInteractionControllerProvider
    {
        public IOwnerInteractionController InteractionController => OwnerPlayerComponents?.InteractionController;
        public IInteractionObserver InteractionObserver => OwnerPlayerComponents?.InteractionController;

        public OwnerPlayerComponents OwnerPlayerComponents { get; private set; }

        public event Action<OwnerPlayerComponents> CreatedOwnerPlayer;

        private readonly ConfigsProviderService _configsProvider;
        private readonly CameraFactory _cameraFactory;
        private readonly HashedAssetProvider _hashedAssetProvider;
        private readonly CharacterControllerPushablesFactory _characterControllerPushablesFactory;
        private readonly ClientsPlayersFactory _clientsPlayersFactory;
        private readonly OwnerPlayerRepository _ownerPlayerRepository;
        private readonly DamageReceiversRepository _damageReceiversesRepository;
        private readonly IAssetLoader _assetLoader;
        private readonly IsOperatorRepository _isOperatorRepository;
        private readonly UpdateObserversService _updateObserversService;

        public OwnerPlayerFactory(
            HashedAssetProvider hashedAssetProvider,
            ConfigsProviderService configsProvider,
            CameraFactory cameraFactory,
            CharacterControllerPushablesFactory characterControllerPushablesFactory,
            ClientsPlayersFactory clientsPlayersFactory, OwnerPlayerRepository ownerPlayerRepository,
            DamageReceiversRepository damageReceiversesRepository, IAssetLoader assetLoader,
            IsOperatorRepository isOperatorRepository, UpdateObserversService updateObserversService)
        {
            _hashedAssetProvider = hashedAssetProvider;
            _configsProvider = configsProvider;
            _cameraFactory = cameraFactory;
            _characterControllerPushablesFactory = characterControllerPushablesFactory;
            _clientsPlayersFactory = clientsPlayersFactory;
            _ownerPlayerRepository = ownerPlayerRepository;
            _damageReceiversesRepository = damageReceiversesRepository;
            _assetLoader = assetLoader;
            _isOperatorRepository = isOperatorRepository;
            _updateObserversService = updateObserversService;
        }

        public async UniTask DisposeAsync()
        {
            OwnerPlayerComponents = null;
            _ownerPlayerRepository.OwnerPlayerComponents = null;
            await _hashedAssetProvider.DisposeAsync();
        }

        public async UniTask<OwnerPlayerComponents> GetCreatedOwnerPlayerComponentsAsync(
            NetworkConnection networkConnection,
            OwnerPlayerSerializableComponents ownerSerializableComponents = null,
            ClientsPlayersFactory.ClientPlayerInitializationData? initializationData = null, Vector3? position = null)
        {
            if (OwnerPlayerComponents != null)
                return OwnerPlayerComponents;

            ownerSerializableComponents ??=
                (await _clientsPlayersFactory.GetCreatedClientPlayerSerializableComponentsAsync(networkConnection,
                    position))
                .GetComponent<OwnerPlayerSerializableComponents>();

            var cameraComponents = await _cameraFactory.GetCreatedMainCameraAsync();

            var clientComponents = await _clientsPlayersFactory.GetCreatedClientPlayerComponentsAsync(
                networkConnection,
                ownerSerializableComponents.ClientSerializableComponents,
                initializationData);

            await ConfigureOwnerComponentsAsync(ownerSerializableComponents, cameraComponents, clientComponents);

            OwnerPlayerComponents.SerializableComponents.gameObject.GetOrAddComponent<DestroyObserver>().Destroyed +=
                MakeNullOwnerPlayerComponents;

            CreatedOwnerPlayer?.Invoke(OwnerPlayerComponents);

            return OwnerPlayerComponents;

            void MakeNullOwnerPlayerComponents()
            {
                OwnerPlayerComponents = null;
            }
        }

        private async UniTask ConfigureOwnerComponentsAsync(
            OwnerPlayerSerializableComponents ownerSerializableComponents,
            CameraComponents cameraComponents, ClientPlayerComponents clientComponents)
        {
            ConfigureCamera(ownerSerializableComponents.transform);
            ConfigureCollider(clientComponents.SerializableComponents.CharacterControllerCollider);
            ConfigureItemParent(clientComponents.SerializableComponents.ItemParentTransform,
                -ownerSerializableComponents.CameraFollow.transform.localPosition);

            if (_isOperatorRepository.IsOperator)
            {
                return;
            }

            var ownerObjectResolver = await BuildOwnerContainerAsync(cameraComponents, ownerSerializableComponents,
                clientComponents);

            OwnerPlayerComponents = ownerObjectResolver.Resolve<OwnerPlayerComponents>();
            _ownerPlayerRepository.OwnerPlayerComponents = OwnerPlayerComponents;

            ownerObjectResolver.Resolve<OwnerPlayerStateMachineInitializer>().Initialize(
                new PlayerStateMachineBuilder<IFeetOwnerPlayerState>(
                    OwnerPlayerComponents.FeetStateMachineModel, ownerObjectResolver),
                new PlayerStateMachineBuilder<IHandsOwnerPlayerState>(OwnerPlayerComponents.HandsStateMachineModel,
                    ownerObjectResolver), cameraComponents.SerializableComponents[CameraParentType.Shake]);

            ownerObjectResolver.Resolve<OwnerPlayerControllersMediator>().Subscribe();

            clientComponents.ViewComponents.SerializableComponents.HealthBar.gameObject.SetActive(false);

            return;

            void ConfigureCamera(Transform playerTransform)
            {
                _updateObserversService.TryAddOrGetUpdateObserver(playerTransform.gameObject, UpdateType.LateUpdate,
                    out var updateObserver);
                
                cameraComponents.FollowPositionController.SetParameters(
                    updateObserver,
                    ownerSerializableComponents.CameraFollow,
                    () => Vector3.zero, isLocalOffset: true);

                cameraComponents.FollowRotationController.SetParameters(
                    updateObserver,
                    ownerSerializableComponents.CameraFollow,
                    () => Vector3.zero, false,
                    true,
                    false);

                cameraComponents.FollowPositionController.StartFollow();
                cameraComponents.FollowRotationController.StartFollow();
            }

            void ConfigureCollider(Collider collider)
            {
                collider.enabled = false;
            }

            void ConfigureItemParent(Transform itemParent, Vector3 offsetFromCamera)
            {
                _updateObserversService.TryAddOrGetUpdateObserver(itemParent.gameObject, UpdateType.LateUpdate,
                    out var updateObserver);
                
                new FollowPositionController(
                    updateObserver,
                    itemParent,
                    cameraComponents.SerializableComponents[CameraParentType.Move],
                    () => offsetFromCamera, isLocalOffset: true);

                new FollowRotationController(
                    updateObserver,
                    itemParent,
                    cameraComponents.SerializableComponents[CameraParentType.Move], () => Vector3.zero);
            }
        }

        private async UniTask<IObjectResolver> BuildOwnerContainerAsync(
            CameraComponents cameraComponents,
            OwnerPlayerSerializableComponents ownerSerializableComponents,
            ClientPlayerComponents clientComponents)
        {
            var lifetimeScope = ownerSerializableComponents.gameObject.GetComponent<OwnerPlayerLifetimeScope>();

            var pushHandlerModelContainer = new DataContainer<CharacterControllerPushHandlerModel>();

            var pushHandler = await _characterControllerPushablesFactory.TryCreateCharacterControllerPushHandlerAsync(
                ownerSerializableComponents.GetComponent<ExplodableSerializableComponents>(), pushHandlerModelContainer,
                false);
            
            lifetimeScope.SetDependenciesAndPrepareToBuild(
                new OwnerPlayerLifetimeScope.Dependencies
                {
                    InventoryItemsConfig = await _configsProvider.GetConfigAsync<InventoryItemsConfig>(),
                    MovementConfig = await _configsProvider.GetConfigAsync<MovementConfig>(),
                    PhysicsLayersConfig = await _configsProvider.GetConfigAsync<PhysicsLayersConfig>(),
                    InventoryItemsModel = clientComponents.InventoryItemsModel,
                    CameraComponents = cameraComponents,
                    PlayerTransform = ownerSerializableComponents.transform,
                    OwnerPlayerSerializableComponents = ownerSerializableComponents,
                    ClientPlayerComponents = clientComponents,
                    DamageReceiversRepository = _damageReceiversesRepository,
                    PushHandlerController = pushHandler,
                    PushHandlerModel = pushHandlerModelContainer.Data
                }
            );

            await lifetimeScope.CustomBuildAsync();
            return lifetimeScope.Container;
        }
    }
}