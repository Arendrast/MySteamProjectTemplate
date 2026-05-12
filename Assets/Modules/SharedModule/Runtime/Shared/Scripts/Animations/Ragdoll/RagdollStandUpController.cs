using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Animations.Ragdoll
{
    public class RagdollStandUpController
    {
        private bool IsFrontUp => Vector3.Dot(_hipsBone.up, Vector3.up) > 0;

        private readonly Transform _parent;
        private readonly Transform _hipsBone;

        private readonly RigAdjusterForAnimation _rigAdjusterForBackStandingUpAnimation;
        private readonly RigAdjusterForAnimation _rigAdjusterForFrontStandingUpAnimation;

        public RagdollStandUpController(Transform parent, Animator animator, int backStandUpClip,
            int frontStandClip)
        {
            _parent = parent;
            _hipsBone = animator.GetBoneTransform(HumanBodyBones.Hips);

            var bones = _hipsBone.GetComponentsInChildren<Transform>();

            _rigAdjusterForBackStandingUpAnimation =
                new RigAdjusterForAnimation(backStandUpClip, bones, animator);
            _rigAdjusterForFrontStandingUpAnimation =
                new RigAdjusterForAnimation(frontStandClip, bones, animator);
        }

        public void AppointBonesToStartAnimation()
        {
            _rigAdjusterForBackStandingUpAnimation.AppointBonesToStartAnimation();
        }


        public void PlayStandUp(Action<int> playStandUp)
        {
            if (IsFrontUp)
                _rigAdjusterForFrontStandingUpAnimation.Adjust(() =>
                    CallbackForAdjustStandingUpAnimation(playStandUp, true));
            else
                _rigAdjusterForBackStandingUpAnimation.Adjust(() =>
                    CallbackForAdjustStandingUpAnimation(playStandUp, false));
        }

        private void CallbackForAdjustStandingUpAnimation(Action<int> playAnimationFunc, bool frontStandingUp)
        {
            playAnimationFunc?.Invoke(frontStandingUp
                ? _rigAdjusterForFrontStandingUpAnimation.AnimationId
                : _rigAdjusterForBackStandingUpAnimation.AnimationId);
        }

        private void AdjustParentPositionToHipsBone()
        {
            var initHipsPosition = _hipsBone.position;
            _parent.position = initHipsPosition;

            _hipsBone.position = initHipsPosition;
        }

        private void AdjustParentRotationToHipsBone()
        {
            var initHipsPosition = _hipsBone.position;
            var initHipsRotation = _hipsBone.rotation;

            var directionForRotate = _hipsBone.up;

            if (!IsFrontUp)
                directionForRotate *= -1;

            directionForRotate.y = 0;

            var correctionRotation = Quaternion.FromToRotation(_parent.forward, directionForRotate.normalized);
            _parent.rotation *= correctionRotation;

            _hipsBone.position = initHipsPosition;
            _hipsBone.rotation = initHipsRotation;
        }
    }
}