using System;
using Cysharp.Threading.Tasks;
using ProjectName.ItemModule.Runtime.Shared.Scripts.Logic;

namespace ProjectName.ItemModule.Runtime.Shared.Scripts.View
{
    public abstract class ConcreteItemsViewFactory<TItemViewConfig, TItemModel> : IConcreteItemsViewFactory<TItemViewConfig, TItemModel>
        where TItemViewConfig : class, IItemViewConfig where TItemModel : class, IItemModel
    {
        public async UniTask InitializeInstanceAsync(ItemViewSerializableComponents instance, IItemViewConfig itemViewConfig,
            IItemModel itemModel) =>
            await InitializeConcreteControllerAsync(instance, (TItemViewConfig) itemViewConfig,
                (TItemModel) itemModel);

        public abstract UniTask InitializeConcreteControllerAsync(ItemViewSerializableComponents instance,
            TItemViewConfig viewConfig, TItemModel canModel);
        
        public Type GetItemViewConfigType()
        {
            return typeof(TItemViewConfig);
        }

        public Type GetItemModelType()
        {
            return typeof(TItemModel);
        }
    }
}