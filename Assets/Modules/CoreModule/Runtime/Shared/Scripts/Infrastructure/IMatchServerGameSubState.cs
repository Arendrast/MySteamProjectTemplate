using System;
using Cysharp.Threading.Tasks;
using Modules.LevelModule.Runtime.Shared.Scripts;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public interface IMatchServerGameSubState : IDisposable
    {
        UniTask EnterAsync(string sceneName, LevelConfig levelConfig);
    }
}