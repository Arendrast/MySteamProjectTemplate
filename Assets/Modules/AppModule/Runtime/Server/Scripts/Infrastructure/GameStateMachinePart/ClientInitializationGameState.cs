using Cysharp.Threading.Tasks;
using FishNet.Connection;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;

namespace Modules.AppModule.Runtime.Server.Scripts.Infrastructure.GameStateMachinePart
{
    public class ClientInitializationGameState : IGameState, IMatchServerService
    {
        public UniTask EnterAsync(NetworkConnection networkConnection)
        {
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(NetworkConnection networkConnection) => UniTask.CompletedTask;
    }
}