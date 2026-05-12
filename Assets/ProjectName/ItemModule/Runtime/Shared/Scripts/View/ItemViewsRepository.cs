using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.ItemModule.Runtime.Shared.Scripts.View
{
    public class ItemViewsRepository : IndexRepository<int, ItemViewSerializableComponents>, IMatchSharedService
    {
        
    }
}