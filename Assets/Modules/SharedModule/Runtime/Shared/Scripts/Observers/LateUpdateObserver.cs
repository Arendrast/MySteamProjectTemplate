using System;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers
{
    public class LateUpdateObserver
    {
        public event Action LateUpdated;

        public void LateUpdate()
        {
            LateUpdated?.Invoke();
        }
    }
}