using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public class InteractablesRepository : IndexRepository<InteractableSerializableComponents, IInteractable>, IMatchSharedService
    {
        
    }
}