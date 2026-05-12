using Modules.SharedModule.Runtime.Shared.Scripts.Repository;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.ItemModule.Runtime.Shared.Scripts.View
{
    public class ItemViewsRepository : IndexRepository<int, ItemViewSerializableComponents>, IMatchSharedService
    {
        
    }
}