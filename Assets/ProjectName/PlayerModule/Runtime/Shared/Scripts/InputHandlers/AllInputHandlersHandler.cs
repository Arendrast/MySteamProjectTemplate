using System.Collections.Generic;
using System.Linq;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class AllInputHandlersHandler : IOwnerPlayerComponent
    {
        private bool _doesHavePriorityUpdater;
        private readonly TimeScaleRepository _timeScaleRepository;

        private static readonly IEnumerable<PlayerInputHandlerType> _sharedInputHandlers = new PlayerInputHandlerType[]
        {
            PlayerInputHandlerType.SwitchCursorMode,
        };

        private readonly Dictionary<PlayerInputHandlerType, IPlayerInputHandler> _inputHandlersDictionary;
        private readonly ActiveInputHandlersTypesRepository _activeInputHandlersTypesRepository;

        public AllInputHandlersHandler(IEnumerable<IPlayerInputHandler> inputHandlers,
            TimeScaleRepository timeScaleRepository,
            ActiveInputHandlersTypesRepository activeInputHandlersTypesRepository)
        {
            _timeScaleRepository = timeScaleRepository;
            _activeInputHandlersTypesRepository = activeInputHandlersTypesRepository;
            _inputHandlersDictionary = inputHandlers.ToDictionary(
                inputHandler => inputHandler.GetInputHandlerType(), inputHandler => inputHandler);
        }

        public void SetDoesHavePriorityUpdater(bool doesHavePriorityUpdater)
        {
            _doesHavePriorityUpdater = doesHavePriorityUpdater;
        }

        public void TryUpdateSelectedHandlers(PlayerInputHandlerType[] inputHandlersTypes, bool isPriority = false)
        {
            if (_timeScaleRepository.IsTimeScaleZero() || !isPriority && _doesHavePriorityUpdater)
                return;

            if (!inputHandlersTypes.SequenceEqual(_activeInputHandlersTypesRepository.ActiveInputHandlerTypes))
            {
                var exitedHandlesTypes =
                    _activeInputHandlersTypesRepository.ActiveInputHandlerTypes.Except(inputHandlersTypes);

                foreach (var handlerType in exitedHandlesTypes)
                {
                    if (_inputHandlersDictionary.TryGetValue(handlerType, out var handler) &&
                        handler is IExitablePlayerInputHandler exitablePlayerInputHandler)
                    {
                        exitablePlayerInputHandler.Exit(); // Should it use? Maybe
                    }
                }

                _activeInputHandlersTypesRepository.SetActiveInputHandlerTypes(inputHandlersTypes
                    .Concat(_sharedInputHandlers).Distinct()
                    .ToList()); // Critical allocation context? Should test;
            }

            foreach (var handlerType in _activeInputHandlersTypesRepository.ActiveInputHandlerTypes)
            {
                TryUpdateInputHandler(handlerType);
            }
        }

        private void TryUpdateInputHandler(PlayerInputHandlerType handlerType)
        {
            if (_inputHandlersDictionary.TryGetValue(handlerType, out var inputHandler))
            {
                inputHandler.Update();
            }
            else
            {
                Debug.LogError($"Input handler by {handlerType} is not found!");
            }
        }
    }
}