using System;
using System.Collections.Generic;
using System.Linq;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Sirenix.Utilities;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class AllInputHandlersHandler : IOwnerPlayerComponent, IDisposable
    {
        private bool _doesHavePriorityUpdater;

        private readonly HashSet<PlayerInputHandlerType> _temporaryPlayerNewInputHandlersTypes =
            new HashSet<PlayerInputHandlerType>();

        private readonly Dictionary<PlayerInputHandlerType, IPlayerInputHandler> _inputHandlersDictionary;
        private readonly ActiveInputHandlersTypesRepository _activeInputHandlersTypesRepository;

        private static readonly PlayerInputHandlerType[] _sharedInputHandlers = new PlayerInputHandlerType[]
        {
            PlayerInputHandlerType.SwitchCursorMode,
        };

        public AllInputHandlersHandler(IEnumerable<IPlayerInputHandler> inputHandlers,
            ActiveInputHandlersTypesRepository activeInputHandlersTypesRepository)
        {
            _activeInputHandlersTypesRepository = activeInputHandlersTypesRepository;
            _inputHandlersDictionary = inputHandlers.ToDictionary(
                inputHandler => inputHandler.GetInputHandlerType(), inputHandler => inputHandler);
        }
        
        public void Dispose()
        {
            _activeInputHandlersTypesRepository.ActiveInputHandlerTypes.ForEach(TryUnsubscribeInputHandler);
            _temporaryPlayerNewInputHandlersTypes.Clear();
        }

        public void SetDoesHavePriorityUpdater(bool doesHavePriorityUpdater)
        {
            _doesHavePriorityUpdater = doesHavePriorityUpdater;
        }

        public void SubscribeNewInputHandlers(PlayerInputHandlerType[] newInputHandlersTypes, bool isPriority = false)
        {
            if (!isPriority && _doesHavePriorityUpdater)
                return;

            if (!newInputHandlersTypes.SequenceEqual(_activeInputHandlersTypesRepository.ActiveInputHandlerTypes))
            {
                _temporaryPlayerNewInputHandlersTypes.Clear();
                _temporaryPlayerNewInputHandlersTypes.AddRange(_sharedInputHandlers);
                _temporaryPlayerNewInputHandlersTypes.AddRange(newInputHandlersTypes);
                
                UnsubscribeOldInputHandlers();
                SubscribeNewInputHandlers();
                
                _activeInputHandlersTypesRepository.SetActiveInputHandlerTypes(_temporaryPlayerNewInputHandlersTypes);
            }

            return;

            void SubscribeNewInputHandlers()
            {
                foreach (var newInputHandlerType in _temporaryPlayerNewInputHandlersTypes)
                {
                    if (!_activeInputHandlersTypesRepository.ActiveInputHandlerTypes.Contains(newInputHandlerType) &&
                        _inputHandlersDictionary.TryGetValue(newInputHandlerType, out var inputHandler))
                    {
                        inputHandler.SetSubscribeState(SubscribeState.Subscribe);
                    }
                }
            }

            void UnsubscribeOldInputHandlers()
            {
                foreach (var oldInputHandlerType in _activeInputHandlersTypesRepository.ActiveInputHandlerTypes)
                {
                    if (!_temporaryPlayerNewInputHandlersTypes.Contains(oldInputHandlerType))
                    {
                        TryUnsubscribeInputHandler(oldInputHandlerType);
                    }
                }
            }
        }

        private void TryUnsubscribeInputHandler(PlayerInputHandlerType inputHandlerType)
        {
            if (_inputHandlersDictionary.TryGetValue(inputHandlerType, out var inputHandler))
            {
                inputHandler.SetSubscribeState(SubscribeState.Unsubscribe);
            }
        }
    }
}