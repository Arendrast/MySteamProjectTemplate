using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using Modules.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.MainMenuPopup;
using Modules.SharedModule.Runtime.Client.Scripts.UI;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using Object = UnityEngine.Object;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Tests
{
    public static class Setup
    {
        private const float MaxEnterToMatchGameStateTime = 5f;

        public static async Task MatchGameState(bool asHost)
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
    }
}