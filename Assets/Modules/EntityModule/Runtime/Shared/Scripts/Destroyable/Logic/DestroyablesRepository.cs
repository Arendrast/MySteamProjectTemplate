using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.SharedModule.Runtime.Shared.Scripts.Repository;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Destroyable.Logic
{
    public class DestroyablesRepository : IndexRepository<DestroyableSerializableComponents, HealthModel>, IMatchSharedService
    {
        
    }
}