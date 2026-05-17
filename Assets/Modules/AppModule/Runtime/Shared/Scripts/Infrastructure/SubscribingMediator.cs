using Cysharp.Threading.Tasks;
using FishNet.Managing.Client;
using Modules.AppModule.Runtime.Shared.Scripts.GameStateMachinePart;
using Modules.SharedModule.Runtime.Client.Scripts.GameStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using VContainer.Unity;

namespace Modules.AppModule.Runtime.Shared.Scripts.Infrastructure
{
    public class SubscribingMediator : IStartable
    {
        public SubscribingMediator(
            EventBus eventBus,
            GameStateMachine gameStateMachine, ClientManager clientManager)
        {
            eventBus.Subscribe<EnterGameStateEvent>(@event =>
                gameStateMachine.TryEnterStateAsync(@event).Forget());
        }

        public void Start()
        {
        }
    }
}