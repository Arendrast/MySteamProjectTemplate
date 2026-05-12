using System;
using Cysharp.Threading.Tasks;
using ProjectName.LevelModule.Runtime.Shared.Scripts;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public interface IMatchClientGameSubState : IDisposable
    {
        UniTask EnterAsync(bool isOperator, string hostSteamId, string sceneName);
    }
}