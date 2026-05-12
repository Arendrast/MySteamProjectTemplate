using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using MoreLinq;
using ProjectName.EntityModule.Runtime.Shared.Scripts;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View.EffectReactorsView;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;
using ProjectName.InventoryModule.Runtime.Shared.Scripts;
using ProjectName.InventoryModule.Runtime.Shared.Scripts.Network;
using ProjectName.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts;
using ProjectName.ItemModule.Runtime.Shared.Scripts.Logic;
using ProjectName.ItemModule.Runtime.Shared.Scripts.View;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer.View;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.Interaction;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;
using ProjectName.SharedModule.Runtime.Shared.Scripts.CameraPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Configs;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Index;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Loading;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Rendering;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer
{
    public class ClientsPlayersFactory : IMatchSharedFactory,
        IClientsInventoryItemsModelsProvider,
        IItemsParentByNetworkConnectionProvider,
        IClientsRemoteInteractionVisitorsProvider,
        IClientsTargetInteractableDataRepositoriesProvider
    {
        #region InitializationData

        public readonly struct ClientPlayerInitializationData
        {
            public readonly InitializeInventoryItemsData InitializeInventoryItemsData;
            public readonly int ViewNetworkObjectId;
            public readonly CharacterType CharacterType;
            public readonly SharedPlayerStateType StateType;
            public readonly InteractableData TargetInteractableData;

            public ClientPlayerInitializationData(
                InitializeInventoryItemsData initializeInventoryItemsData, int viewNetworkObjectId,
                SharedPlayerStateType stateType, InteractableData targetInteractableData, CharacterType characterType)
            {
                ViewNetworkObjectId = viewNetworkObjectId;
                StateType = stateType;
                TargetInteractableData = targetInteractableData;
                CharacterType = characterType;
                InitializeInventoryItemsData = initializeInventoryItemsData;
            }

            public static ClientPlayerInitializationData GetDefault(InventoryItemsConfig inventoryItemsConfig)
            {
                return new ClientPlayerInitializationData(
                    new InitializeInventoryItemsData(inventoryItemsConfig.StartTargetSlotIndex, inventoryItemsConfig
                        .ItemSlotsConfigs
                        .Select(config =>
                            new ItemSlotData(config.DefaultItemConfig?.Id ?? IndexableTools.MissingOrInvalidId,
                                config.DefaultItemConfig != null
                                    ? config.CapacityByItemType.GetValueOrDefault(config.DefaultItemConfig.ItemType)
                                    : 0))
                        .ToArray()),
                    IndexableTools.MissingOrInvalidId, SharedPlayerStateType.Default,
                    default, default);
            }
        }

        #endregion

        public IReadOnlyDictionary<NetworkConnection, INotOwnerInteractionVisitor> NotOwnersInteractionVisitors =>
            ClientsComponentsByNetworkConnection.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.NotOwnerInteractionVisitor);

        public IReadOnlyDictionary<NetworkConnection, Transform> Parents =>
            ClientsComponentsByNetworkConnection.ToDictionary(pair => pair.Key,
                pair => pair.Value.SerializableComponents.ItemParentTransform);

        public IReadOnlyDictionary<NetworkConnection, InventoryItemsModel> ModelByNetworkConnection =>
            ClientsComponentsByNetworkConnection.ToDictionary(pair => pair.Key, pair => pair.Value.InventoryItemsModel);

        public IReadOnlyDictionary<NetworkConnection, TargetInteractableDataRepository> Repositories =>
            ClientsComponentsByNetworkConnection.ToDictionary(pair => pair.Key,
                pair => pair.Value.TargetInteractableDataRepository);

        public IReadOnlyDictionary<NetworkConnection, ClientPlayerComponents> ClientsComponentsByNetworkConnection =>
            _clientsComponentsByNetworkConnection;

        public IReadOnlyDictionary<ClientPlayerSerializableComponents, ClientPlayerComponents>
            ClientPlayerComponentsBySerializableComponents =>
            _clientPlayerComponentsBySerializableComponents;

        public event Action<ClientPlayerComponents, NetworkConnection> CreatedClientPlayer, DespawnedClientPlayer;

        private Dictionary<NetworkConnection, ClientPlayerComponents> _clientsComponentsByNetworkConnection =
            new();

        private readonly List<NetworkConnection> _loadingPlayersConnectins = new List<NetworkConnection>();

        private readonly PlayersTransformsRepository _playersTransformsRepository;


        private readonly Dictionary<ClientPlayerSerializableComponents, ClientPlayerComponents>
            _clientPlayerComponentsBySerializableComponents = new();

        private readonly ConfigsProviderService _configsProvider;
        private readonly CameraFactory _cameraFactory;
        private readonly EntityFactory _entityFactory;
        private readonly ItemsViewFactory _itemsViewFactory;
        private readonly ServerManager _serverManager;
        private readonly ClientManager _clientManager;
        private readonly NetworkManager _networkManager;
        private readonly HashedAssetProvider _hashedAssetProvider;
        private readonly ItemsFactory _itemsFactory;
        private readonly InteractablesRepository _interactablesesRepository;
        private readonly InteractablesFactory _interactablesFactory;
        private readonly InteractableSerializableComponentsFactory _interactableSerializableComponentsFactory;
        private readonly IndexableConfigsProviderService<ItemsConfig, IItemConfig> _itemsConfigsProviderService;

        private readonly AcceptInteractableBroadcastForClientHandleService
            _acceptInteractableBroadcastForClientHandleService;

        private readonly EventBus _eventBus;
        private readonly IAssetLoader _assetLoader;
        private readonly EffectReactorsViewFactory _effectReactorsViewFactory;
        private readonly ItemViewsRepository _itemViewsRepository;

        private const string PlayerViewAssetId = "PlayerView";
        private const string PlayerAssetId = "Player";


        public ClientsPlayersFactory(EntityFactory entityFactory,
            ItemsViewFactory itemsViewFactory,
            ServerManager serverManager,
            ClientManager clientManager,
            NetworkManager networkManager,
            ItemsFactory itemsFactory,
            InteractableSerializableComponentsFactory interactableSerializableComponentsFactory,
            IndexableConfigsProviderService<ItemsConfig, IItemConfig> itemsConfigsProviderService,
            HashedAssetProvider hashedAssetProvider,
            CameraFactory cameraFactory,
            ConfigsProviderService configsProvider,
            InteractablesRepository interactablesesRepository,
            InteractablesFactory interactablesFactory,
            PlayersTransformsRepository playersTransformsRepository,
            AcceptInteractableBroadcastForClientHandleService acceptInteractableBroadcastForClientHandleService,
            EventBus eventBus,
            IAssetLoader assetLoader, EffectReactorsViewFactory effectReactorsViewFactory,
            ItemViewsRepository itemViewsRepository)
        {
            _entityFactory = entityFactory;
            _itemsViewFactory = itemsViewFactory;
            _serverManager = serverManager;
            _clientManager = clientManager;
            _networkManager = networkManager;
            _itemsFactory = itemsFactory;
            _interactableSerializableComponentsFactory = interactableSerializableComponentsFactory;
            _itemsConfigsProviderService = itemsConfigsProviderService;
            _hashedAssetProvider = hashedAssetProvider;
            _cameraFactory = cameraFactory;
            _configsProvider = configsProvider;
            _interactablesesRepository = interactablesesRepository;
            _interactablesFactory = interactablesFactory;
            _playersTransformsRepository = playersTransformsRepository;
            _acceptInteractableBroadcastForClientHandleService = acceptInteractableBroadcastForClientHandleService;
            _eventBus = eventBus;
            _assetLoader = assetLoader;
            _effectReactorsViewFactory = effectReactorsViewFactory;
            _itemViewsRepository = itemViewsRepository;
        }

        public async UniTask<ClientPlayerComponents> TryGetPlayerComponentsAsync(NetworkConnection networkConnection)
        {
            if (networkConnection == null || !networkConnection.IsValid())
                return null;

            await AsyncTools.WaitWhileWithoutSkippingFrame(() => _loadingPlayersConnectins.Contains(networkConnection));

            return _clientsComponentsByNetworkConnection.GetValueOrDefault(networkConnection);
        }

        public async UniTask<InventoryItemsModel> TryGetModel(NetworkConnection networkConnection)
        {
            return (await TryGetPlayerComponentsAsync(networkConnection))?.InventoryItemsModel;
        }

        public NetworkConnection GetNetworkConnection(ClientPlayerComponents clientPlayerComponents)
        {
            return ClientsComponentsByNetworkConnection
                .FirstOrDefault(pair => pair.Value == clientPlayerComponents)
                .Key;
        }

        public async UniTask<ClientPlayerSerializableComponents> GetCreatedClientPlayerSerializableComponentsAsync(
            NetworkConnection networkConnection,
            Vector3? position)
        {
            var prefab =
                await _hashedAssetProvider.GetOrLoadAndRegisterObjectAsync<ClientPlayerSerializableComponents>(
                    PlayerAssetId,
                    shouldCreate: false);

            var instance = Object.Instantiate(prefab, position ?? Vector3.zero, Quaternion.identity);

            _serverManager.TryCustomSpawn(instance.gameObject, ownerConnection: networkConnection);

            return instance;
        }

        public async UniTask<ClientPlayerComponents> GetCreatedClientPlayerComponentsAsync(
            NetworkConnection networkConnection,
            ClientPlayerSerializableComponents serializableComponents,
            ClientPlayerInitializationData? initializationData = null)
        {
            _loadingPlayersConnectins.Add(networkConnection);

            var inventoryItemsConfig = await _configsProvider.GetConfigAsync<InventoryItemsConfig>();
            initializationData ??= ClientPlayerInitializationData.GetDefault(inventoryItemsConfig);

            var clientComponents = await GetCreatedClientComponentsAsync(networkConnection,
                serializableComponents,
                initializationData.Value,
                inventoryItemsConfig);
            await ConfigureClientComponentsAsync(networkConnection,
                serializableComponents,
                initializationData.Value,
                clientComponents,
                inventoryItemsConfig);
            SubscribeClientComponentsAsync(networkConnection, serializableComponents, clientComponents);

            _loadingPlayersConnectins.Remove(networkConnection);

            CreatedClientPlayer?.Invoke(clientComponents, networkConnection);

            return clientComponents;
        }

        public async UniTask DisposeAsync()
        {
            _clientsComponentsByNetworkConnection.Values.ForEach(components => components.Dispose());
            _clientsComponentsByNetworkConnection.Clear();
            _clientPlayerComponentsBySerializableComponents.Clear();
            _playersTransformsRepository.Clear();
            _loadingPlayersConnectins.Clear();
            await _hashedAssetProvider.DisposeAsync();
        }

        private void SubscribeClientComponentsAsync(NetworkConnection networkConnection,
            ClientPlayerSerializableComponents serializableComponents,
            ClientPlayerComponents clientComponents)
        {
            clientComponents.SerializableComponents.gameObject
                .GetOrAddComponent<DestroyObserver>()
                .Destroyed += RemoveComponentsFromList;

            return;

            void RemoveComponentsFromList()
            {
                clientComponents.Dispose();
                _clientsComponentsByNetworkConnection =
                    _clientsComponentsByNetworkConnection.GetWithRemovedItAndAllInvalid(networkConnection);
                _clientPlayerComponentsBySerializableComponents.Remove(serializableComponents);
                _playersTransformsRepository.Remove(clientComponents.SerializableComponents.transform);

                DespawnedClientPlayer?.Invoke(clientComponents, networkConnection);
            }
        }

        private async UniTask ConfigureClientComponentsAsync(NetworkConnection networkConnection,
            ClientPlayerSerializableComponents serializableComponents,
            ClientPlayerInitializationData initializationData,
            ClientPlayerComponents clientComponents,
            InventoryItemsConfig inventoryItemsConfig)
        {
            clientComponents.InventoryItemsModel.StartSetTargetSlot(initializationData
                .InitializeInventoryItemsData.TargetSlotIndex, true);

            await AddItemsToInventorySlotsAsync();

            _clientsComponentsByNetworkConnection.Add(networkConnection, clientComponents);
            _clientPlayerComponentsBySerializableComponents.Add(serializableComponents, clientComponents);
            _playersTransformsRepository.Add(serializableComponents.transform);

            _acceptInteractableBroadcastForClientHandleService.HandleAcceptInteractableBroadcastAsync(
                new AcceptInteractableBroadcastForClient(initializationData.TargetInteractableData, networkConnection),
                NotOwnersInteractionVisitors[networkConnection]);

            clientComponents.StateMachine.EnterState(clientComponents.StateMachine.Nodes
                .First(node => node.Value.State.GetStateType() == initializationData.StateType).Value.State);

            clientComponents.IsFirstEnterPlayerSharedStateRepository.IsFirst = false;

            return;

            async UniTask AddItemsToInventorySlotsAsync()
            {
                for (var slotIndex = 0; slotIndex < inventoryItemsConfig.ItemSlotsAmount; slotIndex++)
                {
                    var itemSlotData = initializationData.InitializeInventoryItemsData.ItemSlotsData[slotIndex];

                    if (itemSlotData.ItemId == IndexableTools.MissingOrInvalidId)
                        continue;

                    for (var i = 0; i < itemSlotData.Count; i++)
                    {
                        var itemModel = await _itemsFactory.GetItemModelAsync(itemSlotData.ItemId,
                            clientComponents.SerializableComponents.ItemParentTransform);

                        clientComponents.InventoryItemsModel.TryAddItemToSlot(itemModel, slotIndex);
                    }
                }
            }
        }

        private async UniTask<ClientPlayerComponents> GetCreatedClientComponentsAsync(
            NetworkConnection networkConnection,
            ClientPlayerSerializableComponents serializableComponents,
            ClientPlayerInitializationData initializationData,
            InventoryItemsConfig inventoryItemsConfig)
        {
            serializableComponents.gameObject.name = $"Player {networkConnection.ClientId}";

            var movementConfig = await _configsProvider.GetConfigAsync<MovementConfig>();

            var entityComponents = await _entityFactory.GetCreatedEntityComponentsAsync(
                serializableComponents.EntityComponents,
                !networkConnection.IsOwner(_clientManager),
                networkConnection.IsOwner(_clientManager));

            var inventoryItemsModel =
                new InventoryItemsModel(serializableComponents.destroyCancellationToken, inventoryItemsConfig);

            var viewComponents = await GetInitializeViewComponentsAsync(networkConnection,
                serializableComponents,
                initializationData,
                entityComponents, inventoryItemsModel);

            var isFirstEnterPlayerSharedStateRepository = new IsFirstEnterPlayerSharedStateRepository
            {
                IsFirst = true
            };

            return new ClientPlayerComponents(
                serializableComponents,
                entityComponents,
                inventoryItemsModel,
                new NotOwnerPlayerInteractionVisitor(),
                viewComponents,
                GetNotOwnerStateMachineModel(),
                new TargetInteractableDataRepository(),
                isFirstEnterPlayerSharedStateRepository,
                networkConnection);
        }

        private FiniteStateMachineModel<IPlayerSharedState> GetNotOwnerStateMachineModel()
        {
            var stateMachineModel = new FiniteStateMachineModel<IPlayerSharedState>();

            var states = new IPlayerSharedState[]
            {
                new PlayerSharedDefaultState(),
            };

            states.ForEach(stateMachineModel.TryAddState);

            return stateMachineModel;
        }

        private async UniTask<PlayerViewComponents> GetInitializeViewComponentsAsync(
            NetworkConnection networkConnection,
            ClientPlayerSerializableComponents serializableComponents,
            ClientPlayerInitializationData initializationData,
            EntityComponents entityComponents, InventoryItemsModel inventoryItemsModel)
        {
            var viewSerializableComponents =
                _serverManager.Started
                    ? await CreateViewAsync(serializableComponents.transform)
                    : _clientManager.TryGetNetworkObjectById(initializationData.ViewNetworkObjectId)
                        .GetComponent<PlayerViewSerializableComponents>();

            _serverManager.TryCustomSpawn(viewSerializableComponents.gameObject, ownerConnection: networkConnection);

            var rigCharacterTypeDataContainer = new DataContainer<CharacterType>();

            var viewRigSerializableComponents =
                viewSerializableComponents.ViewRigSerializableComponents;

            var oldSkinnedMeshRenderers =
                viewRigSerializableComponents.Rig.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            await InitializeViewRigAsync(viewSerializableComponents, rigCharacterTypeDataContainer,
                initializationData.CharacterType);

            foreach (var oldSkinnedMeshRenderer in oldSkinnedMeshRenderers)
            {
                UnityEngine.Object.Destroy(oldSkinnedMeshRenderer.gameObject);
            }

            foreach (var newSkinnedMeshRenderer in viewRigSerializableComponents.Rig
                         .GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                newSkinnedMeshRenderer.updateWhenOffscreen = true;
            }

            _serverManager.TryCustomSpawn(viewRigSerializableComponents.gameObject, ownerConnection: networkConnection);

            if (networkConnection.IsOwner(_clientManager))
            {
                foreach (Transform child in viewRigSerializableComponents.Rig)
                {
                    child.gameObject.SetActive(false);
                }
            }

            var effectableView = viewRigSerializableComponents.EffectableViewSerializableComponents;
            await CreateEffectReactorsViewAsync(entityComponents, effectableView,
                networkConnection.IsOwner(_clientManager), viewSerializableComponents.transform);

            viewRigSerializableComponents.AnimancerComponent.Layers[0].Mask = viewRigSerializableComponents.LegsMask;
            viewRigSerializableComponents.AnimancerComponent.Layers[1].Mask =
                viewRigSerializableComponents.UpperBodyMask;

            viewSerializableComponents.ViewRigSerializableComponents.AnimancerComponent.Animator.Rebind();
            viewRigSerializableComponents.UpperBodyNetworkAnimationPlayer.TryStartPlay();
            viewRigSerializableComponents.LegsNetworkAnimationPlayer.TryStartPlay();

            var cameraComponents = await _cameraFactory.GetCreatedMainCameraAsync();

            viewSerializableComponents.CameraLooker.Construct(cameraComponents.SerializableComponents.Camera);

            var viewComponents = new PlayerViewComponents(
                serializableComponents: viewSerializableComponents,
                viewRigSerializableComponents: viewRigSerializableComponents,
                characterType: rigCharacterTypeDataContainer.Data);

            new InventoryItemsController(inventoryItemsModel, _itemsViewFactory,
                networkConnection.IsOwner(_clientManager),
                viewComponents.ViewRigSerializableComponents.ItemsViewsPositionOrigin, null,
                _itemViewsRepository);

            return viewComponents;
        }

        private async UniTask CreateEffectReactorsViewAsync(EntityComponents entityComponents,
            EffectableViewSerializableComponents effectableView, bool isOwner, Transform parent)
        {
            foreach (var effectReactor in entityComponents.EffectsReceiverModel.EffectReactors.Values)
            {
                foreach (var config in effectableView.EffectReactorsConfigs)
                {
                    var viewReactor =
                        await _effectReactorsViewFactory.GetCreatedEffectReactorAsync(effectReactor, config,
                            effectableView, isOwner);

                    if (viewReactor != null)
                    {
                        break;
                    }
                }
            }
        }

        private async UniTask InitializeViewRigAsync(
            PlayerViewSerializableComponents serializableComponents,
            DataContainer<CharacterType> assetReferenceDataContainer, CharacterType characterType)
        {
            var otherTypes = _clientPlayerComponentsBySerializableComponents.Values
                .Select(component => component.ViewComponents.CharacterType).ToList();

            var pair = _serverManager.Started
                ? serializableComponents.RigsReferences
                    .Where(pair => !otherTypes.Contains(pair.CharacterType))
                    .GetRandomOrDefault()
                : serializableComponents.RigsReferences.FirstOrDefault(rig => rig.CharacterType == characterType);

            assetReferenceDataContainer.Data = pair?.CharacterType ?? default;


            if (pair != null)
            {
                var releasePrefabActionContainer = new DataContainer<Action>();
                var releaseMaterialActionContainer = new DataContainer<Action>();

                var prefab = await AssetProvider.LoadAsync<GameObject>(
                    pair.RigReference, _assetLoader, releasePrefabActionContainer);

                var setSkinnedMeshRenderersController = new SetSkinnedMeshRenderersController(
                    serializableComponents.ViewRigSerializableComponents.SkeletonRoot.parent,
                    serializableComponents.ViewRigSerializableComponents.SkeletonRoot, serializableComponents.transform,
                    await _configsProvider.GetConfigAsync<RenderingLayersConfig>(), () => prefab);

                setSkinnedMeshRenderersController.TryInitialize();
                releasePrefabActionContainer.Data.Invoke();

                foreach (var newSkinnedMeshRenderer in serializableComponents
                             .GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    newSkinnedMeshRenderer.updateWhenOffscreen = true;
#if BAKERY
                    newSkinnedMeshRenderer.gameObject.AddComponent<BakeryVolumeDefaultReceiver>().forceUsage = true;
                    newSkinnedMeshRenderer.gameObject.AddComponent<BakeryVolumeCustomReceiver>();
#endif
                }

                releaseMaterialActionContainer.Data.Invoke();
            }
        }

        private async UniTask<PlayerViewSerializableComponents> CreateViewAsync(Transform parent)
        {
            var prefab =
                await _hashedAssetProvider.GetOrLoadAndRegisterObjectAsync<PlayerViewSerializableComponents>(
                    PlayerViewAssetId,
                    shouldCreate: false
                );

            return Object.Instantiate(prefab, parent);
        }
    }
}