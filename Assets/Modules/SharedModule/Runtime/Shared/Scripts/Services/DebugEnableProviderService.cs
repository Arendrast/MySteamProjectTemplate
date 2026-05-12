using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Services
{
    public class DebugEnableProviderService : IPersistentService, IDisposable
    {
        public bool Enable { get; private set; }
        public event Action EnableChanged;

        private readonly IInputProvider _inputProvider;
        private readonly MonoBehaviourObserver _observer;

        public DebugEnableProviderService(IInputProvider inputProvider, MonoBehaviourObserver observer)
        {
            _inputProvider = inputProvider;
            _observer = observer;
            
            _observer.Updated += Update;
        }

        public void Dispose()
        {
            _observer.Updated -= Update;
        }

        private void Update()
        {
            if (!_inputProvider.IsActionTriggered(InputActionType.Test))
            {
                return;
            }

            Enable = !Enable;
            EnableChanged?.Invoke();
        }
    }
}