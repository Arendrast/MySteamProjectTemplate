using System;
using System.Collections.Generic;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class ActiveInputHandlersTypesRepository : IOwnerPlayerComponent
    {
        public IReadOnlyCollection<PlayerInputHandlerType> ActiveInputHandlerTypes =>
            _activeInputHandlerTypes;

        public event Action<IReadOnlyCollection<PlayerInputHandlerType>> UpdatedActiveInputHandlerTypes;

        private bool _didUpdateAllInputHandlers;
        private IReadOnlyCollection<PlayerInputHandlerType> _activeInputHandlerTypes = new List<PlayerInputHandlerType>();

        public void SetActiveInputHandlerTypes(IReadOnlyCollection<PlayerInputHandlerType> types)
        {
            _activeInputHandlerTypes = types;
            UpdatedActiveInputHandlerTypes?.Invoke(types);
        }
    }
}