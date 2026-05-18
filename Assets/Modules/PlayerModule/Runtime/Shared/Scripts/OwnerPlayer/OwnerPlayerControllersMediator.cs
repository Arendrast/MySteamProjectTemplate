using System;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using UnityEngine.InputSystem;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer
{
    public class OwnerPlayerControllersMediator : IOwnerPlayerComponent, IDisposable
    {
        private readonly OwnerPlayerComponents _ownerPlayerComponents;

        public OwnerPlayerControllersMediator(OwnerPlayerComponents ownerPlayerComponents)
        {
            _ownerPlayerComponents = ownerPlayerComponents;
        }

        public void Dispose()
        {
            
        }

        public void Subscribe()
        {
            SubscribeToControllersEvents(_ownerPlayerComponents);
        }

        private void SubscribeToControllersEvents(OwnerPlayerComponents ownerPlayerComponents)
        {
        }
    }
}