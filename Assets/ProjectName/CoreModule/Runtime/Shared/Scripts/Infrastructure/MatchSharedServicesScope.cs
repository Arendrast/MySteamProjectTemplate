using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using VContainer;
using VContainer.Unity;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public class MatchSharedServicesScope : LifetimeScope
    {
        protected override async UniTask ConfigureAsync(IContainerBuilder builder)
        {
            await builder.RegisterAllInheritorsAsync<IMatchSharedService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MatchSharedDependenciesCreator>();
        }
    }
}