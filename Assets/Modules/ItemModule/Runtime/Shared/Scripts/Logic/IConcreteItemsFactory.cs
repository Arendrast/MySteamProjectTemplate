using System;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;

namespace Modules.ItemModule.Runtime.Shared.Scripts.Logic
{
    public interface IConcreteItemsFactory<in TItemConfig, TCreateConcreteItemData> : IConcreteItemsFactory
        where TItemConfig : IItemConfig where TCreateConcreteItemData : struct
    {
        UniTask<IItemModel> GetConcreteItemModelAsync(TItemConfig itemConfig,
            GameObject gameObjectInstance, TCreateConcreteItemData? createConcreteItemData);
    }

    public interface IConcreteItemsFactory : IMatchSharedFactory
    {
        UniTask<IItemModel> GetItemModelAsync(IItemConfig itemConfig,
            GameObject gameObjectInstance, ICreateConcreteItemData createConcreteItemData);

        Type GetConfigType();
    }
}