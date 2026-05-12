using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Input;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.Movement
{
    public class PlayerMovementStateController : IOwnerPlayerComponent
    {
        private readonly PlayerMovementController _movementController;
        private readonly PlayerRotationController _rotationController;
        private readonly IInputProvider _inputProvider;

        public PlayerMovementStateController(
            PlayerMovementController movementController,
            IInputProvider inputProvider, PlayerRotationController rotationController)
        {
            _movementController = movementController;
            _inputProvider = inputProvider;
            _rotationController = rotationController;
        }

        public bool InAir()
        {
            return !_movementController.IsGrounded;
        }

        public void UpdateAndApplyMovement(bool shouldApplyRotation, Vector2? moveAction = null)
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
                _rotationController.ApplyRotation(_inputProvider.LookAction);
            }
        }
    }
}