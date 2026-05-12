using System;
using Cysharp.Threading.Tasks;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public interface IMatchClientGameSubState : IDisposable
    {
        UniTask EnterAsync(bool isOperator, string hostSteamId, string sceneName);
    }
}