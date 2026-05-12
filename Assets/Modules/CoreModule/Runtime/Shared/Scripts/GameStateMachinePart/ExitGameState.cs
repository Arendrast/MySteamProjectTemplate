using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.GameStateMachine;
using UnityEngine;

namespace Modules.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart
{
    public class ExitGameState : IGameState
    {
        public UniTask EnterAsync(IGameStateEnterData data)
        {
            Application.Quit();
            
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}