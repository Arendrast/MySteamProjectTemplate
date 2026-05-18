using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Input
{
    public interface IInputService
    {
        void SetActiveInputActionsPart(InputActionsPart part, bool isActive);
        Vector2 MoveAction { get; }
        Vector2 LookAction { get; }
        void SetSubscribeStateToInputAction(InputActionType actionType, InputActionPhase phase, Action<InputAction.CallbackContext> action, SubscribeState subscribeState);
        void SetSubscribeStateToInputActionGroup(InputActionGroupType groupType, InputActionPhase phase, Action<InputAction.CallbackContext, int> action, int actionsCount, SubscribeState subscribeState);
    }
}