using Animancer;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Animations
{
    public interface ITransitionsByIdProvider
    {
        ITransition GetTransition(int id);
    }
}