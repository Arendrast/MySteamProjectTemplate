using System;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class RequestSetActiveStateModel
    {
        public bool IsRequestedActive { get; private set; }
        
        public event Action<bool> Requested;
        
        public void SetRequestedSetActiveState(bool isActive)
        {
            IsRequestedActive = isActive;
            Requested?.Invoke(isActive);
        }
    }
}