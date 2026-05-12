using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Input
{
    public sealed class NewInputSystemProvider : IInputProvider, IDisposable
    {
        public Vector2 MoveAction =>
            _timeScaleRepository.IsTimeScaleZero()
                ? Vector2.zero
                : _inputActions.General.Move.ReadValue<Vector2>();

        public Vector2 LookAction => _timeScaleRepository.IsTimeScaleZero()
            ? Vector2.zero
            : _inputActions.General.Look.ReadValue<Vector2>();

        private readonly InputActions _inputActions;
        private readonly TimeScaleRepository _timeScaleRepository;

        // private readonly Dictionary<InputActionType, bool> _pressedButtons = new Dictionary<InputActionType, bool>();
        //
        // private readonly Dictionary<InputActionType, int> _startPressButtonsFrame =
        //     new Dictionary<InputActionType, int>();

        private const string SelectItem = "SelectItem";

        public NewInputSystemProvider(TimeScaleRepository timeScaleRepository,
            InputActions inputActions,
            MonoBehaviourObserver monoBehaviourObserver)
        {
            _timeScaleRepository = timeScaleRepository;
            _inputActions = inputActions;
            _inputActions.Enable();

            /*var allInputActionType = CollectionTools.ParseEnumToList<InputActionType>();

            monoBehaviourObserver.Updated += UpdatePressedButtons;

            return;

            void UpdatePressedButtons()
            {
                foreach (var actionType in allInputActionType)
                {
                    if (IsActionPressed(actionType) &&
                        (!_pressedButtons.ContainsKey(actionType) || !_pressedButtons[actionType]))
                    {
                        _pressedButtons.SetOrAdd(actionType, true);
                        _startPressButtonsFrame.SetOrAdd(actionType, Time.frameCount);
                    }
                }
            }*/
        }

        public bool IsActionPressed(InputActionType actionType)
        {
            return _inputActions.FindAction(actionType.ToString())?.IsPressed() ?? false;
        }

        public bool WasActionReleasedThisFrame(InputActionType actionType) =>
            _inputActions.FindAction(actionType.ToString())?.WasReleasedThisFrame() ?? false;

        public bool IsActionTriggered(InputActionType actionType) => _inputActions.FindAction(actionType.ToString()) is
            { triggered: true };

        public bool IsActionCancelled(InputActionType actionType)
        {
            return _inputActions.FindAction(actionType.ToString()) is
                { phase: InputActionPhase.Canceled };
        }

        public int? GetTriggeredSelectItemIndex(int itemsAmount)
        {
            for (var i = 0; i < itemsAmount; i++)
            {
                var action = _inputActions.FindAction($"{SelectItem}{i}");

                if (action is { triggered: true })
                    return i;
            }

            return null;
        }

        public void Dispose()
        {
            _inputActions?.Dispose();
        }
    }
}