using Animancer;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Animations
{
    public interface ITransitionsByIdProvider
    {
        ITransition GetTransition(int id);
    }
}