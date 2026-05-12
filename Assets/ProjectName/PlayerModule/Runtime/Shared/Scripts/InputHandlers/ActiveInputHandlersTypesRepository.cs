using System;
using System.Collections.Generic;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class ActiveInputHandlersTypesRepository : IOwnerPlayerComponent
    {
        public IReadOnlyList<PlayerInputHandlerType> ActiveInputHandlerTypes =>
            _activeInputHandlerTypes;

        public event Action<IReadOnlyList<PlayerInputHandlerType>> UpdatedActiveInputHandlerTypes;

        private bool _didUpdateAllInputHandlers;
        private IReadOnlyList<PlayerInputHandlerType> _activeInputHandlerTypes = new List<PlayerInputHandlerType>();

        public void SetActiveInputHandlerTypes(IReadOnlyList<PlayerInputHandlerType> types)
        {
            _activeInputHandlerTypes = types;
            UpdatedActiveInputHandlerTypes?.Invoke(types);
        }
    }
}