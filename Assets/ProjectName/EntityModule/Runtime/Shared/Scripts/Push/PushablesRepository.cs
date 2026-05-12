using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push
{
    public class PushablesRepository : IndexRepository<ExplodableSerializableComponents, IPushable>, IMatchSharedService
    {
        
    }
}