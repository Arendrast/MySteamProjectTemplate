using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using VContainer;
using VContainer.Unity;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure
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