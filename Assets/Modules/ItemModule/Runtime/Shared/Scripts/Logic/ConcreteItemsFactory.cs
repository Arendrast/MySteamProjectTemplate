using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.ItemModule.Runtime.Shared.Scripts.Logic
{
    public abstract class ConcreteItemsFactory<TItemConfig, TCreateConcreteItemData> : IConcreteItemsFactory<TItemConfig,
        TCreateConcreteItemData> where TItemConfig : class, IItemConfig where TCreateConcreteItemData : struct
    {
        public abstract UniTask<IItemModel> GetConcreteItemModelAsync(TItemConfig itemConfig,
            GameObject gameObjectInstance, TCreateConcreteItemData? createConcreteItemData);

        public async UniTask<IItemModel> GetItemModelAsync(IItemConfig itemConfig, GameObject gameObjectInstance,
            ICreateConcreteItemData createConcreteItemData)
        {
            return await GetConcreteItemModelAsync((TItemConfig)itemConfig, gameObjectInstance,
                createConcreteItemData is TCreateConcreteItemData data ? data : null);
        }

        public Type GetConfigType()
        {
            return typeof(TItemConfig);
        }
    }
}