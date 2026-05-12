using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.Index;
using Modules.SharedModule.Runtime.Shared.Scripts.Loading;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;

namespace Modules.ItemModule.Runtime.Shared.Scripts.Logic
{
    public class ItemsFactory : IMatchSharedFactory
    {
        private readonly IAssetLoader _assetLoader;
        private readonly IndexableConfigsProviderService<ItemsConfig, IItemConfig> _itemsConfigsProviderService;
        private readonly Dictionary<Type, IConcreteItemsFactory> _concreteItemFactoriesByConfigType;

        public ItemsFactory(IEnumerable<IConcreteItemsFactory> concreteItemFactories,
            IAssetLoader assetLoader,
            IndexableConfigsProviderService<ItemsConfig, IItemConfig> itemsConfigsProviderService)
        {
            _concreteItemFactoriesByConfigType =
                concreteItemFactories.ToDictionary(factory => factory.GetConfigType(), factory => factory);
            _assetLoader = assetLoader;
            _itemsConfigsProviderService = itemsConfigsProviderService;
        }

        public async UniTask<IItemModel> GetItemModelAsync(IItemConfig itemConfig, Transform parent,
            GameObject logicGameObject = null, ICreateConcreteItemData createConcreteItemData = null)
        {
            if (!_concreteItemFactoriesByConfigType.TryGetValue(itemConfig.GetType(), out var concreteFactory))
            {
                return null;
            }

            var itemModel = await concreteFactory.GetItemModelAsync(itemConfig, await GetLogicGameObjectInstance(),
                createConcreteItemData);

            if (itemModel == null)
            {
                return null;
            }

            itemModel.LogicGameObject.transform.parent = parent;

            return itemModel;

            async UniTask<GameObject> GetLogicGameObjectInstance()
            {
                return
                    logicGameObject ?? await AssetProvider.InstantiateAsync<GameObject>(itemConfig.PrefabReference,
                        _assetLoader,
                        parent: parent);
            }
        }

        public async UniTask<IItemModel> GetItemModelAsync(int itemId, Transform parent,
            GameObject logicGameObject = null, ICreateConcreteItemData createConcreteItemData = null)
        {
            var itemConfig = await _itemsConfigsProviderService.GetConfigAsync(itemId);

            if (itemConfig == null)
                Debug.LogError($"Config under {itemId} id is not found in Items Config");

            return itemConfig == null
                ? null
                : await GetItemModelAsync(itemConfig, parent, logicGameObject, createConcreteItemData);
        }
    }
}