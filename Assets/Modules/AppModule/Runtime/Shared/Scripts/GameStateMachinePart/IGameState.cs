using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.GameStateMachine;

namespace Modules.AppModule.Runtime.Shared.Scripts.GameStateMachinePart
{
    public interface IGameState
    {
        UniTask EnterAsync(IGameStateEnterData data);
        UniTask ExitAsync();
    }
}