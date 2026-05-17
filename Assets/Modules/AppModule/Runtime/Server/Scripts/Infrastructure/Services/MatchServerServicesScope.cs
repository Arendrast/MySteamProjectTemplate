using Cysharp.Threading.Tasks;
using Modules.AppModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using VContainer;
using VContainer.Unity;

namespace Modules.AppModule.Runtime.Server.Scripts.Infrastructure.Services
{
    public class MatchServerServicesScope : LifetimeScope, IMatchServerServicesScope
    {
        protected override async UniTask ConfigureAsync(IContainerBuilder builder)
        {
            await builder.RegisterAllInheritorsAsync<IMatchServerService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MatchServerDependenciesCreator>();
        }
    }
}