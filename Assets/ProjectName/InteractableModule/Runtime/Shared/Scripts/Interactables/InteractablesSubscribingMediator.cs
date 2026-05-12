using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public class InteractablesSubscribingMediator : IMatchSharedService
    {
        public InteractablesSubscribingMediator(
            InteractablesFactory factory,
            InteractablesRepository repository)
        {
            repository.Removed += factory.UnsubscribeOnDisable;
        }
    }
}