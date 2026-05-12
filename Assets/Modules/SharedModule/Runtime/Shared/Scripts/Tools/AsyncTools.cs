using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class AsyncTools
    {
        public static async UniTask<bool> AwaitTaskAndGetDoesThrowOperationCancelledException(UniTask func)
        {
            try
            {
                await func;
                return false;
            }
            catch (Exception exception)
            {
                if (exception.IsOperationCanceledException())
                    return true;
                
                throw;
            }
        }
        
        public static async UniTask WaitWhileWithoutSkippingFrame(Func<bool> func, CancellationToken cancellationToken = default)
        {
            var result = func.Invoke();

            if (!result)
                return;
            
            await UniTask.WaitWhile(func, cancellationToken: cancellationToken);
        }
    }
}