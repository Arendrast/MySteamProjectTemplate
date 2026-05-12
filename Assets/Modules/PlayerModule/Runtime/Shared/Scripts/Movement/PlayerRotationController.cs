using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Movement
{
    public class PlayerRotationController : IOwnerPlayerComponent
    {
        private readonly Transform _playerTransform;
        private readonly MovementConfig _config;
        private readonly MouseSensitivityRepository _mouseSensitivityRepository;

        public PlayerRotationController(Transform playerTransform, MovementConfig config, MouseSensitivityRepository mouseSensitivityRepository)
        {
            _playerTransform = playerTransform;
            _config = config;
            _mouseSensitivityRepository = mouseSensitivityRepository;
        }
        
        public void ApplyRotation(Vector2 lookInput)
        {
            if (lookInput == Vector2.zero) return;
            
            var mouseX = lookInput.x * _config.RotationSpeed * _mouseSensitivityRepository.CurrentSensitivity * Time.deltaTime;
            _playerTransform.Rotate(Vector3.up * mouseX);
        }
    }
}