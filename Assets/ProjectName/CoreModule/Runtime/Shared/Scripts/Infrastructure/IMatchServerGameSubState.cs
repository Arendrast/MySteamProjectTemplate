using System;
using Cysharp.Threading.Tasks;
using ProjectName.LevelModule.Runtime.Shared.Scripts;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public interface IMatchServerGameSubState : IDisposable
    {
        UniTask EnterAsync(string sceneName, LevelConfig levelConfig);
    }
}