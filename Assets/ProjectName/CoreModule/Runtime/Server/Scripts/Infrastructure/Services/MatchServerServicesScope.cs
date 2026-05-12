using Cysharp.Threading.Tasks;
using ProjectName.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart;
using ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using VContainer;
using VContainer.Unity;

namespace ProjectName.CoreModule.Runtime.Server.Scripts.Infrastructure.Services
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