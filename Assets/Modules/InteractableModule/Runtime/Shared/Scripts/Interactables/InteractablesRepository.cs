using Modules.SharedModule.Runtime.Shared.Scripts.Repository;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public class InteractablesRepository : IndexRepository<InteractableSerializableComponents, IInteractable>, IMatchSharedService
    {
        
    }
}