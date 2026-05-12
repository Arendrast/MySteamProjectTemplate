using Cysharp.Threading.Tasks;
using FishNet.Managing.Client;
using ProjectName.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart;
using ProjectName.SharedModule.Runtime.Client.Scripts.GameStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using VContainer.Unity;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
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