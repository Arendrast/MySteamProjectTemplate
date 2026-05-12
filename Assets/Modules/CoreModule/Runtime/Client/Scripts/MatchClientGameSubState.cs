using Cysharp.Threading.Tasks;
using FishNet.Managing.Client;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using Modules.SharedModule.Runtime.Client.Scripts.GameStateMachine;
using Modules.SharedModule.Runtime.Client.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.CoreModule.Runtime.Client.Scripts
{
    public class MatchClientGameSubState : IMatchClientGameSubState, IMatchClientService
    {
        private readonly ClientManager _clientManager;
        private readonly EventBus _eventBus;
        private readonly OwnerPlayerFactory _ownerPlayerFactory;
        private readonly NetworkCountersSynchronizerBehaviourRepository _networkCountersSynchronizerBehaviourRepository;
        private readonly ISharedSubscribingMediator _subscribingMediator;

        public MatchClientGameSubState(ClientManager clientManager, EventBus eventBus,
            OwnerPlayerFactory ownerPlayerFactory,
            NetworkCountersSynchronizerBehaviourRepository networkCountersSynchronizerBehaviourRepository,
            ISharedSubscribingMediator subscribingMediator)
        {
            _clientManager = clientManager;
            _eventBus = eventBus;
            _ownerPlayerFactory = ownerPlayerFactory;
            _networkCountersSynchronizerBehaviourRepository = networkCountersSynchronizerBehaviourRepository;
            _subscribingMediator = subscribingMediator;
        }

        public void Dispose()
        {
            Exit();
        }

        public async UniTask EnterAsync(bool isOperator, string hostSteamId, string sceneName)
        {
            await InitializeClientSideAsync(hostSteamId == ""
                ? null
                : hostSteamId, sceneName, _subscribingMediator);

            if (!isOperator)
            {
                await AsyncTools.WaitWhileWithoutSkippingFrame(() =>
                    _ownerPlayerFactory.OwnerPlayerComponents == null);

                _networkCountersSynchronizerBehaviourRepository.Behaviour =
                    UnityEngine.Object.FindFirstObjectByType<NetworkCountersSynchronizerBehaviour>(FindObjectsInactive.Include);
            }
        }

        private UniTask InitializeClientSideAsync(string address, string sceneName,
            ISharedSubscribingMediator subscribingMediator)
        {
            _clientManager.StopConnection();
            if (!_clientManager.StartConnection(address ?? "localhost"))
            {
                _eventBus.Fire(new EnterGameStateEvent(GameStateType.MainMenu));
                return UniTask.CompletedTask;
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

            subscribingMediator.Subscribe();

            return UniTask.CompletedTask;
        }

        private void Exit()
        {
        }
    }
}