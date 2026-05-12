using Modules.SharedModule.Runtime.Shared.Scripts.Repository;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public class PushablesRepository : IndexRepository<ExplodableSerializableComponents, IPushable>, IMatchSharedService
    {
        
    }
}