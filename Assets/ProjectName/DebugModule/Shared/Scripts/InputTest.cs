using CrazySWAT.SharedModule.Runtime.Shared.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectName.DebugModule.Shared.Scripts
{
    public class InputTest : MonoBehaviour
    {
        private ProjectInputActions _projectInputActions;
    
        private void Start()
        {
            _projectInputActions = new ProjectInputActions();
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
