using Cysharp.Threading.Tasks;
using FishNet.Connection;

namespace Modules.CoreModule.Runtime.Server.Scripts.Infrastructure.GameStateMachinePart
{
    public class HostMatchGameState : IGameState
    {
        public UniTask EnterAsync(NetworkConnection networkConnection)
        {
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(NetworkConnection networkConnection)
        {
            return UniTask.CompletedTask;
        }
    }
}