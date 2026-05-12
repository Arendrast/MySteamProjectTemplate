using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Destroyable.Logic
{
    public class DestroyablesRepository : IndexRepository<DestroyableSerializableComponents, HealthModel>, IMatchSharedService
    {
        
    }
}