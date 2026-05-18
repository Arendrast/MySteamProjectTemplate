using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Input
{
    public sealed class NewInputSystemService : IInputService, IDisposable
    {
        public Vector2 MoveAction =>
            _timeScaleRepository.IsTimeScaleZero()
                ? Vector2.zero
                : _inputActions.General.Move.ReadValue<Vector2>();

        public Vector2 LookAction => _timeScaleRepository.IsTimeScaleZero()
            ? Vector2.zero
            : _inputActions.General.Look.ReadValue<Vector2>();

        private readonly Dictionary<InputActionGroupType, IReadOnlyList<InputAction>> _inputActionsGroupsByType =
            new Dictionary<InputActionGroupType, IReadOnlyList<InputAction>>();

        private readonly Dictionary<InputAction, Action<InputAction.CallbackContext>> _actionsByGroupInputAction =
            new Dictionary<InputAction, Action<InputAction.CallbackContext>>();

        private readonly Dictionary<InputActionType, InputAction> _inputActionsByType =
            new Dictionary<InputActionType, InputAction>();

        private readonly InputActions _inputActions;
        private readonly TimeScaleRepository _timeScaleRepository;

        public NewInputSystemService(TimeScaleRepository timeScaleRepository,
            InputActions inputActions)
        {
            _timeScaleRepository = timeScaleRepository;
            _inputActions = inputActions;
            _inputActions.Enable();
        }

        public void Dispose()
        {
            _inputActions?.Dispose();
        }

        public void SetSubscribeStateToInputAction(InputActionType actionType, InputActionPhase phase, Action<InputAction.CallbackContext> action,
            SubscribeState subscribeState)
        {
            Debug.Log(actionType + ": " + subscribeState);
            
            if (subscribeState is SubscribeState.Subscribe)
            {
                SubscribeToInputAction(actionType, phase, action);
            }
            else
            {
                UnsubscribeFromInputAction(actionType, phase, action);
            }
        }

        public void SetSubscribeStateToInputActionGroup(InputActionGroupType groupType, InputActionPhase phase,
            Action<InputAction.CallbackContext, int> action, int actionsCount, SubscribeState subscribeState)
        {
            if (subscribeState is SubscribeState.Subscribe)
            {
                SubscribeToInputActionGroup(groupType, phase, action, actionsCount);
            }
            else
            {
                UnsubscribeFromInputActionGroup(groupType, phase, action, actionsCount);
            }
        }

        private void SubscribeToInputAction(InputActionType actionType, InputActionPhase phase,
            Action<InputAction.CallbackContext> action)
        {
            SubscribeToInputAction(GetInputAction(actionType), phase, action);
        }

        private void UnsubscribeFromInputAction(InputActionType actionType, InputActionPhase phase,
            Action<InputAction.CallbackContext> action)
        {
            UnsubscribeFromInputAction(GetInputAction(actionType), phase, action);
        }

        private void SubscribeToInputActionGroup(InputActionGroupType groupType, InputActionPhase phase,
            Action<InputAction.CallbackContext, int> action, int actionsCount)
        {
            var inputActions = GetGroupInputActions(actionsCount, groupType);

            for (var i = 0; i < inputActions.Count; i++)
            {
                var inputAction = inputActions[i];
                var index = i;
                var resultAction = new Action<InputAction.CallbackContext>((context) => action?.Invoke(context, index));
                _actionsByGroupInputAction[inputAction] = resultAction; 

                SubscribeToInputAction(inputAction, phase, resultAction);
            }
        }

        private void UnsubscribeFromInputActionGroup(InputActionGroupType groupType, InputActionPhase phase,
            Action<InputAction.CallbackContext, int> action, int actionsCount)
        {
            var inputActions = GetGroupInputActions(actionsCount, groupType);

            foreach (var inputAction in inputActions)
            {
                if (_actionsByGroupInputAction.TryGetValue(inputAction, out var resultAction))
                {
                    UnsubscribeFromInputAction(inputAction, phase, resultAction);   
                }
            }
        }

        private InputAction GetInputAction(InputActionType actionType)
        {
            if (!_inputActionsByType.TryGetValue(actionType, out var inputAction))
            {
                inputAction = _inputActions.FindAction(actionType.ToString());

                if (inputAction != null)
                {
                    _inputActionsByType.Add(actionType, inputAction);
                }
            }

            return inputAction;
        }

        public IReadOnlyList<InputAction> GetGroupInputActions(int actionsCount, InputActionGroupType groupType)
        {
            if (_inputActionsGroupsByType.TryGetValue(groupType, out var inputActions))
            {
                return inputActions;
            }

            var actions = new List<InputAction>();
            var groupTypeString = groupType.ToString();

            for (var i = 0; i < actionsCount; i++)
            {
                var inputAction = _inputActions.FindAction($"{groupTypeString}{i}");

                if (inputAction == null)
                {
                    continue;
                }

                actions.Add(inputAction);
            }

            _inputActionsGroupsByType.Add(groupType, actions);

            return actions;
        }

        private void SubscribeToInputAction(InputAction inputAction, InputActionPhase phase,
            Action<InputAction.CallbackContext> action)
        {
            if (inputAction == null)
            {
                return;
            }

            switch (phase)
            {
                case InputActionPhase.Started:
                    inputAction.started += action;
                    break;
                case InputActionPhase.Performed:
                    inputAction.performed += action;
                    break;
                case InputActionPhase.Canceled:
                    inputAction.canceled += action;
                    break;
            }
        }

        private void UnsubscribeFromInputAction(InputAction inputAction, InputActionPhase phase,
            Action<InputAction.CallbackContext> action)
        {
            if (inputAction == null)
            {
                return;
            }

            switch (phase)
            {
                case InputActionPhase.Started:
                    inputAction.started -= action;
                    break;
                case InputActionPhase.Performed:
                    inputAction.performed -= action;
                    break;
                case InputActionPhase.Canceled:
                    inputAction.canceled -= action;
                    break;
            }
        }
    }
}