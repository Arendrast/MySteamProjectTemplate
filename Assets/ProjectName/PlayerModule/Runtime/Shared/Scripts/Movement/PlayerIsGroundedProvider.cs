using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using ProjectName.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.Movement
{
    public class PlayerIsGroundedProvider : IOwnerPlayerComponent
    {
        private readonly IsGroundedProvider _isGroundedProvider;
        private readonly FiniteStateMachineModel<IFeetOwnerPlayerState> _feetStateMachine;

        public PlayerIsGroundedProvider(IsGroundedProvider isGroundedProvider,
            OwnerPlayerSerializableComponents clientPlayerSerializableComponents,
            FiniteStateMachineModel<IFeetOwnerPlayerState> feetStateMachine)
        {
            _isGroundedProvider = isGroundedProvider;
            _feetStateMachine = feetStateMachine;

            clientPlayerSerializableComponents.GetOrAddComponent<MonoBehaviourObserver>().DrawedGizmos +=
                _isGroundedProvider.DrawGizmos;
        }

        public IsGroundedProvider GetBaseProvider() => _isGroundedProvider;
        public bool IsGrounded()
        {
            return _isGroundedProvider.IsGrounded();
        }

        public RaycastHit GetGroundHitUnderFeet(out bool changedHitCollider)
        {
            return _isGroundedProvider.GetGroundHitUnderFeet(out changedHitCollider);
        }

        public RaycastHit GetGroundHitUnderFeet()
        {
            return _isGroundedProvider.GetGroundHitUnderFeet();
        }
    }
}