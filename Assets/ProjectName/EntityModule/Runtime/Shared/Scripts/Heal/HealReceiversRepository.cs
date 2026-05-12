using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Heal
{
    public class HealReceiversRepository : IndexRepository<int, HealReceiverModel>, IMatchSharedService
    {
        
    }
}