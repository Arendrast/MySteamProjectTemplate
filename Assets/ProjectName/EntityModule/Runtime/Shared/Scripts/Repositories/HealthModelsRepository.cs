using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Repositories
{
    public class HealthModelsRepository : IndexRepository<int, HealthModel>, IMatchSharedService
    {
    }
}