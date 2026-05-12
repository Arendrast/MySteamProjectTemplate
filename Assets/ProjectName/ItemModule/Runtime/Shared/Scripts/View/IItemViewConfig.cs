using ProjectName.SharedModule.Runtime.Shared.Scripts.Index;

namespace ProjectName.ItemModule.Runtime.Shared.Scripts.View
{
    public interface IItemViewConfig : IIndexable
    {
        SharedItemViewConfig SharedItemViewConfig { get; }
    }
}