using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.GameStateMachine;
using UnityEngine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart
{
    public class HardResetGameState : IGameState
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