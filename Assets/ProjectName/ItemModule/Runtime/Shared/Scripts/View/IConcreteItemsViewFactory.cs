using System;
using Cysharp.Threading.Tasks;
using ProjectName.ItemModule.Runtime.Shared.Scripts.Logic;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.ItemModule.Runtime.Shared.Scripts.View
{
    public interface IConcreteItemsViewFactory : IMatchSharedFactory
    {
        UniTask InitializeInstanceAsync(ItemViewSerializableComponents instance, IItemViewConfig itemViewConfig, IItemModel itemModel);
        Type GetItemViewConfigType();
        Type GetItemModelType();
    }
    
    public interface IConcreteItemsViewFactory<TItemViewConfig, TItemModel> : IConcreteItemsViewFactory
    {
        UniTask InitializeConcreteControllerAsync(ItemViewSerializableComponents instance, TItemViewConfig itemViewConfig, TItemModel canModel);
    }
}