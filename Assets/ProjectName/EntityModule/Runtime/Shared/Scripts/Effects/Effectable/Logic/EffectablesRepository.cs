using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic
{
    public class EffectablesRepository : IndexRepository<EffectableSerializableComponents, IEffectable>, IMatchSharedService
    {
        
    }
}