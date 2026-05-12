using System;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Push;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network;
using ProjectName.InventoryModule.Runtime.Shared.Scripts;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.DependencyInjection;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.Operator;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.StateMachinePart;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.HandsStates;
using ProjectName.SharedModule.Runtime.Shared.Scripts.CameraPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Configs;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Loading;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using ProjectName.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using VContainer;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer
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

        public OwnerPlayerFactory(
            HashedAssetProvider hashedAssetProvider,
            ConfigsProviderService configsProvider,
            CameraFactory cameraFactory,
            CharacterControllerPushablesFactory characterControllerPushablesFactory,
            ClientsPlayersFactory clientsPlayersFactory, OwnerPlayerRepository ownerPlayerRepository,
            DamageReceiversRepository damageReceiversesRepository, IAssetLoader assetLoader,
            IsOperatorRepository isOperatorRepository)
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
                cameraComponents.FollowPositionController.SetParameters(
                    playerTransform.gameObject.GetOrAddComponent<MonoBehaviourObserver>(),
                    ownerSerializableComponents.CameraFollow,
                    () => Vector3.zero, isLocalOffset: true);

                cameraComponents.FollowRotationController.SetParameters(
                    playerTransform.gameObject.GetOrAddComponent<MonoBehaviourObserver>(),
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
                new FollowPositionController(
                    itemParent.gameObject.GetOrAddComponent<MonoBehaviourObserver>(),
                    itemParent,
                    cameraComponents.SerializableComponents[CameraParentType.Move],
                    () => offsetFromCamera, isLocalOffset: true);

                new FollowRotationController(
                    itemParent.gameObject.GetOrAddComponent<MonoBehaviourObserver>(),
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