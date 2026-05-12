using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Input
{
    public interface IInputProvider 
    {
        Vector2 MoveAction { get; }
        Vector2 LookAction { get; }
        bool IsActionPressed(InputActionType actionType);
        bool WasActionReleasedThisFrame(InputActionType actionType);
        bool IsActionTriggered(InputActionType actionType);
        int? GetTriggeredSelectItemIndex(int itemsAmount);
    }
}