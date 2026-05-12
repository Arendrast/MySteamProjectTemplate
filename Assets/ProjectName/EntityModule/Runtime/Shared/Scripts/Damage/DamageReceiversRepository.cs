using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Damage
{
    public class DamageReceiversRepository : IndexRepository<int, DamageReceiverModel>, IMatchSharedService
    {
        
    }
}