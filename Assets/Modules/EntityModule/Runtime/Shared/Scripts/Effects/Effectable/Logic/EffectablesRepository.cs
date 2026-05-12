using Modules.SharedModule.Runtime.Shared.Scripts.Repository;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic
{
    public class EffectablesRepository : IndexRepository<EffectableSerializableComponents, IEffectable>, IMatchSharedService
    {
        
    }
}