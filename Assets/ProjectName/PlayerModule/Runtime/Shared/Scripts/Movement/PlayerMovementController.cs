using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.Movement
{
    public class PlayerMovementController : IOwnerPlayerComponent
    {
        public bool IsGrounded => _isGroundedProvider.IsGrounded();
        
        private readonly PlayerIsGroundedProvider _isGroundedProvider;

        public PlayerMovementController(PlayerIsGroundedProvider isGroundedProvider)
        {
            _isGroundedProvider = isGroundedProvider;
        }

        public void UnRequestDelayedJump()
        {
            
        }
    }
}