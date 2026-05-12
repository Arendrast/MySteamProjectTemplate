using Cysharp.Threading.Tasks;
using FishNet.Connection;
using ProjectName.CoreModule.Runtime.Server.Scripts.Infrastructure.Services;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.CoreModule.Runtime.Server.Scripts.Infrastructure.GameStateMachinePart
{
    public interface IGameState : IMatchServerService
    {
        UniTask EnterAsync(NetworkConnection networkConnection);
        UniTask ExitAsync(NetworkConnection networkConnection);
    }
}