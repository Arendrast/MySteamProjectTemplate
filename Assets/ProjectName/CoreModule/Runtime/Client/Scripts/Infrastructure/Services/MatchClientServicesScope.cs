using Cysharp.Threading.Tasks;
using ProjectName.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart;
using ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.NetworkTimer;
using ProjectName.SharedModule.Runtime.Client.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.SubscribingMediators;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using VContainer;
using VContainer.Unity;

namespace ProjectName.CoreModule.Runtime.Client.Scripts.Infrastructure.Services
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