using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
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