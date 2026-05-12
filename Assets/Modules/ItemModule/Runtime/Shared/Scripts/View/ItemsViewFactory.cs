using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.ItemModule.Runtime.Shared.Scripts.Logic;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.Loading;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Rendering;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using Sirenix.Utilities;
using UnityEngine;

namespace Modules.ItemModule.Runtime.Shared.Scripts.View
{
    public class ItemsViewFactory : IDisposable, IMatchSharedFactory
    {
        private readonly IAssetLoader _assetLoader;
        private readonly ConfigsProviderService _configsProviderService;
        private readonly ItemViewsRepository _viewsInstancesByLogicGameObject;

        private readonly Dictionary<Type, IConcreteItemsViewFactory[]> _concreteItemsViewFactoriesByItemViewConfigType;
        private readonly Dictionary<Type, IConcreteItemsViewFactory> _concreteItemsViewFactoriesByItemModelType;
        private readonly HashSet<GameObject> _loadingViewInstanceByLogicGameObject = new HashSet<GameObject>();

        public ItemsViewFactory(IEnumerable<IConcreteItemsViewFactory> concreteItemFactories, IAssetLoader assetLoader,
            ConfigsProviderService configsProviderService,
            ItemViewsRepository viewsInstancesByLogicGameObject)
        {
            _assetLoader = assetLoader;

            var concreteItemFactoriesArray = concreteItemFactories.ToArray();

            _concreteItemsViewFactoriesByItemModelType =
                concreteItemFactoriesArray.ToDictionary(factory => factory.GetItemModelType(), factory => factory);

            _concreteItemsViewFactoriesByItemViewConfigType = GetConcreteItemFactoriesByItemViewConfigType();

            _configsProviderService = configsProviderService;
            _viewsInstancesByLogicGameObject = viewsInstancesByLogicGameObject;

            return;

            Dictionary<Type, IConcreteItemsViewFactory[]> GetConcreteItemFactoriesByItemViewConfigType()
            {
                var itemViewConfigTypes = concreteItemFactoriesArray.Select(factory => factory.GetItemViewConfigType())
                    .Distinct().ToArray();

                return itemViewConfigTypes.ToDictionary(
                    type => type,
                    type => concreteItemFactoriesArray.Where(factory =>
                        type == factory.GetItemViewConfigType()).ToArray());
            }
        }

        public void Dispose() => _loadingViewInstanceByLogicGameObject.Clear();

        public async UniTask<ItemViewSerializableComponents> GetItemViewInstanceAsync(IItemModel itemModel,
            Transform positionOrigin, Transform rotationOrigin, bool isOwner, Transform parent)
        {
            var itemViewConfig = await GetItemViewConfigAsync(itemModel);
            return itemViewConfig == null
                ? null
                : await GetItemViewInstanceAsync(itemViewConfig, positionOrigin, rotationOrigin, itemModel, isOwner,
                    parent);
        }

        private async UniTask<ItemViewSerializableComponents> GetItemViewInstanceAsync(IItemViewConfig itemViewConfig,
            Transform positionOrigin, Transform rotationOrigin,
            IItemModel itemModel, bool isOwner, Transform parent)
        {
            if (itemModel == null || itemModel.LogicGameObject == null)
            {
                return null;
            }

            if (_viewsInstancesByLogicGameObject.ValueByKey.TryGetValue(itemModel.LogicGameObject.GetInstanceID(),
                    out var instance))
            {
                return instance;
            }

            if (!_loadingViewInstanceByLogicGameObject.Add(itemModel.LogicGameObject))
            {
                await UniTask.WaitWhile(() =>
                    !_loadingViewInstanceByLogicGameObject.Contains(itemModel.LogicGameObject));
                return _viewsInstancesByLogicGameObject.ValueByKey.GetValueOrDefault(itemModel.LogicGameObject
                    .GetInstanceID());
            }

            instance = await InstantiateViewInstanceAsync(itemViewConfig, _assetLoader, parent);
            await TryInitializeViewInstanceAsync(itemViewConfig, instance, itemModel, isOwner, positionOrigin,
                rotationOrigin);

            instance.transform.localPosition = instance.LocalPosition;
            instance.transform.localRotation = Quaternion.Euler(instance.LocalRotation);

            return instance;
        }

        private async UniTask TryInitializeViewInstanceAsync(IItemViewConfig itemViewConfig,
            ItemViewSerializableComponents instance,
            IItemModel itemModel, bool isOwner, Transform positionOrigin, Transform rotationOrigin)
        {
            if (isOwner)
                instance.gameObject.SetActive(false);

            var instanceId = itemModel.LogicGameObject.GetInstanceID();

            var layersConfig = await _configsProviderService.GetConfigAsync<RenderingLayersConfig>();

            instance.GetComponentsInChildren<SkinnedMeshRenderer>().ForEach(meshRenderer =>
            {
                meshRenderer.renderingLayerMask = layersConfig[RenderingLayerGroup.RealtimeLight];
#if BAKERY
                meshRenderer.gameObject.AddComponent<BakeryVolumeDefaultReceiver>().forceUsage = true;
                meshRenderer.gameObject.AddComponent<BakeryVolumeCustomReceiver>();
#endif
            });

            _viewsInstancesByLogicGameObject.Add(instanceId, instance);
            _loadingViewInstanceByLogicGameObject.Remove(itemModel.LogicGameObject);

            if (!_concreteItemsViewFactoriesByItemViewConfigType.TryGetValue(itemViewConfig.GetType(),
                    out var factoriesByConfigType) ||
                !_concreteItemsViewFactoriesByItemModelType.TryGetValue(itemModel.GetType(),
                    out var factoryByModelType) || !factoriesByConfigType.Contains(factoryByModelType))
                return;

            if (isOwner)
            {
                await factoryByModelType.InitializeInstanceAsync(instance, itemViewConfig, itemModel);
            }

            itemModel.LogicGameObject.GetOrAddComponent<DestroyObserver>().Destroyed += DestroyInstance;
            instance.GetOrAddComponent<DestroyObserver>().Destroyed += RemoveItemViewFromRepository;

            return;

            void DestroyInstance()
            {
                UnityEngine.Object.Destroy(instance.gameObject);
            }

            void RemoveItemViewFromRepository()
            {
                _viewsInstancesByLogicGameObject.RemoveByKey(instanceId);
            }
        }

        public async UniTask<IItemViewConfig> GetItemViewConfigAsync(IItemModel itemModel)
        {
            var itemViewConfig =
                (await _configsProviderService.GetConfigAsync<ItemsViewConfig>()).ItemConfigs.FirstOrDefault(config =>
                    config.Id == itemModel.Config.Id);

            if (itemViewConfig == null)
                Debug.LogError($"Config under {itemModel.Config.Id} is not found in Items View Config");

            return itemViewConfig;
        }

        private async UniTask<ItemViewSerializableComponents> InstantiateViewInstanceAsync(
            IItemViewConfig itemViewConfig,
            IAssetLoader assetLoader, Transform parent)
        {
            return await AssetProvider.InstantiateAsync<ItemViewSerializableComponents>(
                itemViewConfig.SharedItemViewConfig.PrefabReference, assetLoader, parent: parent);
        }
    }
}