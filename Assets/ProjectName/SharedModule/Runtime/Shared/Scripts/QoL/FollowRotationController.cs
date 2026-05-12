using System;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class FollowRotationController
    {
        public MonoBehaviourObserver Observer { get; private set; }
        public Transform FollowerTransform { get; private set; }
        public Transform TargetTransform { get; private set; }
        public bool FollowX { get; private set; }
        public bool FollowY { get; private set; }
        public bool FollowZ { get; private set; }
        public bool InvertFollowX { get; private set; }
        public bool InvertFollowY { get; private set; }
        public bool InvertFollowZ { get; private set; }
        public bool IsLocalSpace { get; private set; }
        public Func<Vector3> OffsetFunc { get; private set; }

        public FollowRotationController(MonoBehaviourObserver observer,
            Transform followerTransform,
            Transform targetTransform,
            Func<Vector3> offsetFunc,
            bool followX = true,
            bool followY = true,
            bool followZ = true, bool shouldStartFollow = true, bool isLocalSpace = false,
            bool invertFollowX = false,
            bool invertFollowY = false,
            bool invertFollowZ = false)
        {
            SetParameters(observer, targetTransform, offsetFunc, followX, followY, followZ, isLocalSpace, invertFollowX,
                invertFollowY, invertFollowZ);
            
            FollowerTransform = followerTransform;

            if (shouldStartFollow)
                StartFollow();
        }

        public FollowRotationController(Transform followerTransform)
        {
            FollowerTransform = followerTransform;
        }

        public void SetParameters(MonoBehaviourObserver observer,
            Transform targetTransform,
            Func<Vector3> offsetFunc,
            bool followX = true,
            bool followY = true,
            bool followZ = true, bool isLocalSpace = false,
            bool invertFollowX = false,
            bool invertFollowY = false,
            bool invertFollowZ = false)
        {
            IsLocalSpace = isLocalSpace;
            Observer = observer;
            TargetTransform = targetTransform;
            FollowX = followX;
            FollowY = followY;
            FollowZ = followZ;
            OffsetFunc = offsetFunc;
            InvertFollowX = invertFollowX;
            InvertFollowY = invertFollowY;
            InvertFollowZ = invertFollowZ;
        }

        public void StartFollow()
        {
            EndFollow();
            Observer.LateUpdated += Follow;
        }

        public void EndFollow()
        {
            if (Observer != null)
                Observer.LateUpdated -= Follow;
        }

        public Quaternion GetTargetRotation()
        {
            var targetRotation = IsLocalSpace
                ? TargetTransform.localRotation.eulerAngles + OffsetFunc.Invoke()
                : TargetTransform.rotation.eulerAngles + OffsetFunc.Invoke();

            var followerRotation = IsLocalSpace
                ? FollowerTransform.localRotation.eulerAngles
                : FollowerTransform.rotation.eulerAngles;

            return Quaternion.Euler(new Vector3(
                FollowX ? (InvertFollowX ? -targetRotation.x : targetRotation.x) : followerRotation.x,
                FollowY ? (InvertFollowY ? -targetRotation.y : targetRotation.y) : followerRotation.y,
                FollowZ ? (InvertFollowZ ? -targetRotation.z : targetRotation.z) : followerRotation.z));
        }

        public void Follow()
        {
            if (FollowerTransform == null || TargetTransform == null)
            {
                EndFollow();
                return;
            }

            var resultRotation = GetTargetRotation();

            if (IsLocalSpace)
            {
                FollowerTransform.localRotation = resultRotation;
            }
            else
            {
                FollowerTransform.rotation = resultRotation;
            }
        }
    }
}