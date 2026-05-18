using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class FollowPositionController
    {
        public Transform FollowerTransform { get; private set; }
        public Transform TargetTransform { get; private set; }
        public Func<Vector3> OffsetFunc { get; private set; }
        public bool FollowX { get; private set; }
        public bool FollowY { get; private set; }
        public bool FollowZ { get; private set; }
        public bool IsLocalOffset { get; private set; }
        public UpdateObserver Observer { get; private set; }

        public FollowPositionController(UpdateObserver updateObserver,
            Transform followerTransform,
            Transform targetTransform,
            Func<Vector3> offsetFunc,
            bool followX = true,
            bool followY = true,
            bool followZ = true,
            bool shouldStartFollow = true,
            bool isLocalOffset = false)
        {
            FollowerTransform = followerTransform;
            TargetTransform = targetTransform;
            OffsetFunc = offsetFunc;
            FollowX = followX;
            FollowY = followY;
            FollowZ = followZ;
            IsLocalOffset = isLocalOffset;
            Observer = updateObserver;

            if (shouldStartFollow)
                StartFollow();
        }

        public FollowPositionController(Transform followerTransform)
        {
            FollowerTransform = followerTransform;
        }

        public void SetOffset(Func<Vector3> offset)
        {
            OffsetFunc = offset;
        }
        
        public void SetObserver(UpdateObserver observer)
        {
            Observer = observer;
        }

        public void SetTargetTransform(Transform targetTransform)
        {
            TargetTransform = targetTransform;
        }

        public void SetParameters(UpdateObserver updateObserver,
            Transform targetTransform,
            Func<Vector3> offsetFunc,
            bool followX = true,
            bool followY = true,
            bool followZ = true,
            bool isLocalOffset = false)
        {
            Observer = updateObserver;
            TargetTransform = targetTransform;
            OffsetFunc = offsetFunc;
            FollowX = followX;
            FollowY = followY;
            FollowZ = followZ;
            IsLocalOffset = isLocalOffset;
        }

        public void StartFollow()
        {
            EndFollow();

            if (Observer.UpdateType != UpdateType.LateUpdate)
            {
                Debug.LogWarning($"Update observer for {Observer.GameObject.name} does not use LateUpdate");
            }
            
            Observer.Updated += Follow;
        }

        public void EndFollow()
        {
            if (Observer != null)
                Observer.Updated -= Follow;
        }

        public Vector3 GetTargetPosition()
        {
            var offsettedPosition = IsLocalOffset
                ? TargetTransform.TransformPoint(OffsetFunc.Invoke())
                : TargetTransform.transform.position + OffsetFunc.Invoke();

            var followerPosition = FollowerTransform.position;

           return new Vector3(
                FollowX ? offsettedPosition.x : followerPosition.x,
                FollowY ? offsettedPosition.y : followerPosition.y,
                FollowZ ? offsettedPosition.z : followerPosition.z);
        }

        public void Follow(float time)
        {
            if (FollowerTransform == null || TargetTransform == null)
            {
                EndFollow();
                return;
            }
            
            FollowerTransform.transform.position = GetTargetPosition();
        }
    }
}