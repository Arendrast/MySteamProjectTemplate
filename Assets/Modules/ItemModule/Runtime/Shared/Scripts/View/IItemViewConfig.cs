using Modules.SharedModule.Runtime.Shared.Scripts.Index;

namespace Modules.ItemModule.Runtime.Shared.Scripts.View
{
    public interface IItemViewConfig : IIndexable
    {
        SharedItemViewConfig SharedItemViewConfig { get; }
    }
}