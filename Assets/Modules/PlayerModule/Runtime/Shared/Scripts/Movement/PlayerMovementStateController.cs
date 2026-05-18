using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Movement
{
    public class PlayerMovementStateController : IOwnerPlayerComponent
    {
        private readonly PlayerMovementController _movementController;
        private readonly PlayerRotationController _rotationController;
        private readonly IInputService _inputService;

        public PlayerMovementStateController(
            PlayerMovementController movementController,
            IInputService inputService, PlayerRotationController rotationController)
        {
            _movementController = movementController;
            _inputService = inputService;
            _rotationController = rotationController;
        }

        public bool InAir()
        {
            return !_movementController.IsGrounded;
        }

        public void UpdateAndApplyMovement(bool shouldApplyRotation, float time, Vector2? moveAction = null)
        {
            //_movementController.UpdateGravity();
            
            //_movementController.UpdateMovement(moveAction ?? _inputProvider.MoveAction);

            //_movementController.ApplyMovement();

            if (shouldApplyRotation)
                TryApplyRotation();
        }

        public void TryApplyRotation()
        {
            if (!CursorSwitchTools.IsCursorEnabled)
            {
                _rotationController.ApplyRotation(_inputService.LookAction);
            }
        }
    }
}