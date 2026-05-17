using Cysharp.Threading.Tasks;
using Modules.AppModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Client.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using VContainer;
using VContainer.Unity;

namespace Modules.AppModule.Runtime.Client.Scripts.Infrastructure.Services
{
    public class MatchClientServicesScope : LifetimeScope, IMatchClientServicesScope
    {
        protected override async UniTask ConfigureAsync(IContainerBuilder builder)
        {
            await builder.RegisterAllInheritorsAsync<IMatchClientService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MatchClientDependenciesCreator>();
        }
    }
}