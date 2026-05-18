using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using UnityEngine.InputSystem;
using InputActionType = Modules.SharedModule.Runtime.Shared.Scripts.Input.InputActionType;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Services
{
    public class DebugEnableProviderService : IPersistentService, IDisposable
    {
        public bool Enable { get; private set; }
        public event Action EnableChanged;

        private readonly IInputService _inputService;

        public DebugEnableProviderService(IInputService inputService)
        {
            _inputService = inputService;
            SetSubscribeState(SubscribeState.Subscribe);
        }

        public void Dispose()
        {
            SetSubscribeState(SubscribeState.Unsubscribe);
        }

        private void SetSubscribeState(SubscribeState subscribeState)
        {
            _inputService.SetSubscribeStateToInputAction(InputActionType.Test, InputActionPhase.Started, SetActiveDebug, subscribeState);
        }

        private void SetActiveDebug(InputAction.CallbackContext context)
        {
            Enable = !Enable;
            EnableChanged?.Invoke();
        }
    }
}