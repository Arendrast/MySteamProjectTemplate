using System;
using System.Collections;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using FishNet;
using Modules.AppModule.Runtime.Shared.Scripts.GameStateMachinePart;
using Modules.AppModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.AppModule.Runtime.Shared.Scripts.Systems.UISystem.MainMenuPopup;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using Modules.SharedModule.Runtime.Client.Scripts.UI;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Modules.AppModule.Runtime.Shared.Scripts.Tests
{
    public static class Setup
    {
        private const float MaxEnterToMatchGameStateTime = 5f;

        public static IEnumerator MatchGameState(bool asHost)
        {
            return MatchGameStateAsync(asHost).ToCoroutine();
        }

        public static IEnumerable RestartPlayMode()
        {
            if (Application.isPlaying)
            {
                yield return new ExitPlayMode();    
            }
            
            yield return new EnterPlayMode();
        }

        public static IEnumerator WaitForConnectAnotherPlayerAndInvokeAction(Action<ClientPlayerComponents> action)
        {
            var playerFactory = LifetimeScope.Find<MatchSharedServicesScope>().Container
                .Resolve<ClientsPlayersFactory>();

            yield return new WaitWhile(() => playerFactory.ClientsComponentsByNetworkConnection.Count < 2);

            var playerClientComponents =
                playerFactory.ClientsComponentsByNetworkConnection.FirstOrDefault(player =>
                    !player.Key.IsOwner(InstanceFinder.ClientManager)).Value;
            
            action?.Invoke(playerClientComponents);
        }
        
        public static async UniTask MatchGameStateAsync(bool asHost)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

            await UniTask.WaitWhile(() => !asyncLoad.isDone);

            var scope = await Object.FindFirstObjectByType<PersistentServicesScopeLoader>()
                .TryCreatePersistentServicesScopeAsync();

            await scope.TryConfigureAsync();

            var gameStateMachine = scope.Container.Resolve<GameStateMachine>();

            var token = new CancellationTokenSource(TimeSpan.FromSeconds(MaxEnterToMatchGameStateTime));

            await UniTask.WaitWhile(() => gameStateMachine.ActiveGameState is not ClientMainMenuGameState,
                cancellationToken: token.Token);

            var mainMenuPopupController = await
                scope.Container.Resolve<MainMenuPopupFactory>().GetMainMenuPopupControllerAsync();

            if (!UITools.TrySimulateRealClick(asHost
                    ? mainMenuPopupController.PopupSerializableComponents.StartAsHostButton
                    : mainMenuPopupController.PopupSerializableComponents.StartAsClientButton))
            {
                throw new Exception("Cant touch start as host button");
            }

            var matchGameState = scope.Container.Resolve<MatchGameState>();

            await UniTask.WaitWhile(() => !matchGameState.EndedEnter, cancellationToken: token.Token);
        }

        public static OwnerPlayerComponents OwnerPlayer()
        {
            return LifetimeScope.Find<MatchSharedServicesScope>().Container.Resolve<OwnerPlayerFactory>()
                .OwnerPlayerComponents;
        }

        public static EffectApplierController EffectApplierController()
        {
            return LifetimeScope.Find<MatchSharedServicesScope>().Container
                .Resolve<EffectApplierFactory>().GetCreatedEffectApplierController(
                    new GameObject().AddComponent<EffectApplierSerializableComponents>(),
                    EffectType.None, 0, 0, 0, new GameObject().AddComponent<BoxOverlapObserver>());
        }
    }
}