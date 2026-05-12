using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.GameStateMachine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart
{
    public interface IGameState
    {
        UniTask EnterAsync(IGameStateEnterData data);
        UniTask ExitAsync();
    }
}