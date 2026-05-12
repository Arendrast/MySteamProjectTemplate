using System;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class InvokeOnDispose : IDisposable
    {
        private readonly Action _action;

        public InvokeOnDispose(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action?.Invoke();
        }
    }
}