using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Movement
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