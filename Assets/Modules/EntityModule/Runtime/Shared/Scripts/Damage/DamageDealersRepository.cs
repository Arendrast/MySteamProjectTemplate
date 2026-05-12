using Modules.SharedModule.Runtime.Shared.Scripts.Repository;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Damage
{
    public class DamageDealersRepository : IndexRepository<int, DamageDealerModel>, IMatchSharedService
    {
        
    }
}