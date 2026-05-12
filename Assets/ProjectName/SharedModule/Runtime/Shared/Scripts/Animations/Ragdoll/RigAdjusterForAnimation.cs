using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Animations.Ragdoll
{
    public class RigAdjusterForAnimation
    {
        public int AnimationId { get; }
        
        private CancellationTokenSource _shiftBonesToStandingUpAnimationTokenSource;
        private readonly Animator _animator;

        private readonly List<Transform> _bones;
        private readonly BoneTransformData[] _bonesBeforeAnimation;
        private readonly BoneTransformData[] _bonesAtStartAnimation;
        
        private const float TimeToShiftBonesToStartAnimation = 0.5f;

        public RigAdjusterForAnimation(int animationId, IEnumerable<Transform> bones, Animator animator)
        {
            AnimationId = animationId;
            _animator = animator;
            _bones = new List<Transform>(bones);

            _bonesBeforeAnimation = new BoneTransformData[_bones.Count];
            //_bonesAtStartAnimation = bonesAtStartAnimation;

            for (var i = 0; i < _bones.Count; i++)
            {
                _bonesBeforeAnimation[i] = new BoneTransformData();
            }
        }
        
        public void AppointBonesToStartAnimation()
        {
            for (var i = 0; i < _bones.Count; i++)
            {
                _bones[i].transform.localPosition = _bonesAtStartAnimation[i].Position;
                _bones[i].transform.localRotation = _bonesAtStartAnimation[i].Rotation;
            }
        }

        public void Adjust(Action endClipCallback)
        {
            SaveCurrentBonesDataTo(_bonesBeforeAnimation);

            RecreateTokenSource();

            ShiftBonesToAnimationAsync(endClipCallback).Forget();
        }

        private void RecreateTokenSource()
        {
            _shiftBonesToStandingUpAnimationTokenSource?.Cancel();
            _shiftBonesToStandingUpAnimationTokenSource?.Dispose();

            _shiftBonesToStandingUpAnimationTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(new CancellationTokenSource().Token,
                    _animator.gameObject.GetCancellationTokenOnDestroy());
        }

        private async UniTask ShiftBonesToAnimationAsync(Action endClipCallback)
        {
            /*float progress = 0;

            while (progress < TimeToShiftBonesToStartAnimation)
            {
                progress += Time.deltaTime;
                var progressInPercantage = progress / TimeToShiftBonesToStartAnimation;

                for (var i = 0; i < _bones.Count; i++)
                {
                    _bones[i].localPosition = Vector3.Lerp(_bonesBeforeAnimation[i].Position,
                        _bonesAtStartAnimation[i].Position, progressInPercantage);
                    _bones[i].localRotation = Quaternion.Lerp(_bonesBeforeAnimation[i].Rotation,
                        _bonesAtStartAnimation[i].Rotation, progressInPercantage);
                }

                if (await AsyncTools.AwaitTaskAndGetDoesThrowOperationCancelledException(UniTask.DelayFrame(1,
                        cancelImmediately: true,
                        cancellationToken: _shiftBonesToStandingUpAnimationTokenSource.Token)))
                {
                    _shiftBonesToStandingUpAnimationTokenSource.Dispose();
                    _shiftBonesToStandingUpAnimationTokenSource = null;
                    return;
                }
            }*/

            endClipCallback?.Invoke();
        }

        private void SaveCurrentBonesDataTo(BoneTransformData[] bones)
        {
            for (var i = 0; i < bones.Length; i++)
            {
                bones[i].Position = _bones[i].localPosition;
                bones[i].Rotation = _bones[i].localRotation;
            }
        }
    }
}