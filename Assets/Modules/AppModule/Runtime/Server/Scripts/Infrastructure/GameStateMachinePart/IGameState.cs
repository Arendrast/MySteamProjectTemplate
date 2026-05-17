using Cysharp.Threading.Tasks;
using FishNet.Connection;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;

namespace Modules.AppModule.Runtime.Server.Scripts.Infrastructure.GameStateMachinePart
{
    public interface IGameState : IMatchServerService
    {
        UniTask EnterAsync(NetworkConnection networkConnection);
        UniTask ExitAsync(NetworkConnection networkConnection);
    }
}