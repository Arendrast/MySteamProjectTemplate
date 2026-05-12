using Modules.SharedModule.Runtime.Shared.Scripts.Repository;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Heal
{
    public class HealReceiversRepository : IndexRepository<int, HealReceiverModel>, IMatchSharedService
    {
        
    }
}