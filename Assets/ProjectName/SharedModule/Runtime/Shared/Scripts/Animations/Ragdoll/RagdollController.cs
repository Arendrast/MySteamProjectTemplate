using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Animations.Ragdoll
{
    public class RagdollController
    {
        public IReadOnlyList<Rigidbody> RagdollableRigidbodies => _rigidbodies;
        
        private readonly Animator _animator;
        private readonly Rigidbody[] _rigidbodies;
        private readonly RagdollStandUpController _ragdollStandUpController;
        
        public RagdollController(Animator animator, RagdollStandUpController ragdollStandUpController)
        {
            _animator = animator;
            _ragdollStandUpController = ragdollStandUpController;
            _rigidbodies = animator.GetComponentsInChildren<Rigidbody>(true);
            Disable();
        }

        public void Hit(Vector3 force, Vector3 hitPosition)
        {
            Fall(true);
            var injuredRigidbody = _rigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPosition))
                .First();
            injuredRigidbody.AddForceAtPosition(force, hitPosition, ForceMode.Impulse);
        }

        public void StandUp(Action<int> playStandUpAnimation)
        {
            Disable();
            _ragdollStandUpController.PlayStandUp(playStandUpAnimation);
        }

        public void AppointBonesToStartAnimation()
        {
            _ragdollStandUpController.AppointBonesToStartAnimation();
        }

        public void Fall(bool withGravity, bool firstKinematic = false)
        {
            Enable(withGravity, firstKinematic);
        }

        private void Enable(bool withGravity, bool firstKinematic = false)
        {
            var isFirst = true;
            
            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = withGravity;

                if (isFirst)
                {
                    rigidbody.isKinematic = firstKinematic;
                    isFirst = false;
                }
            }
            
            _animator.enabled = false;
        }

        public void Disable()
        {
            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
            }
            
            _animator.enabled = true;
        }
    }
}