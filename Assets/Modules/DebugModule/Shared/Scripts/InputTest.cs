using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.DebugModule.Shared.Scripts
{
    public class InputTest : MonoBehaviour
    {
        private InputActions _projectInputActions;
    
        private void Start()
        {
            _projectInputActions = new InputActions();
            _projectInputActions.Enable();
        }

        private void Update()
        {
            var moveAction = _projectInputActions.General.Move.ReadValue<Vector2>();
        
            Debug.Log(Keyboard.current.wKey.isPressed);
        
            if (Keyboard.current.wKey.isPressed && moveAction == Vector2.zero)
            {
                Debug.LogWarning($"[FRAME {Time.frameCount}] КОНФЛИКТ: Кнопка зажата, но Action в нуле!");
            }
        }
    }
}
