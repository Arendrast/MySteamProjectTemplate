using System;
using System.Linq;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.PhysicsPart
{
    public class IsGroundedProvider
    {
        private RaycastHit _lastHit;
        private int _lastCheckFrame;
        private bool _changedHitCollider;

        private readonly PhysicsLayersConfig _physicsLayersConfig;
        private readonly Func<Vector3> _centerFunc;
        private readonly Func<float> _radiusFunc;
        private readonly Func<float> _overridedMaxDistanceFunc;
        private readonly RaycastHit[] _results = new RaycastHit[1024];
        private readonly Func<RaycastHit[], int, RaycastHit> _raycastHitFilter;

        public IsGroundedProvider(PhysicsLayersConfig physicsLayersConfig,
            Func<Vector3> centerFunc, Func<float> radiusFunc,
            Func<RaycastHit[], int, RaycastHit> raycastHitFilter, Func<float> overridedMaxDistanceFunc = null)
        {
            _physicsLayersConfig = physicsLayersConfig;
            _centerFunc = centerFunc;
            _radiusFunc = radiusFunc;
            _overridedMaxDistanceFunc = overridedMaxDistanceFunc;
            _raycastHitFilter = raycastHitFilter ?? GetFirstRaycastHit;
        }
        
        public Vector3 GetCenter()
        {
            return _centerFunc.Invoke();
        }

        public Func<RaycastHit[], int, RaycastHit> GetRaycastHitFilter()
        {
            return _raycastHitFilter;
        }

        public float GetRadius()
        {
            return _radiusFunc.Invoke();
        }

        public bool IsGrounded()
        {
            return GetGroundHitUnderFeet().collider != null;
        }

        public RaycastHit GetGroundHitUnderFeet()
        {
            return GetGroundHitUnderFeet(out var changedCollider);
        }

        public void DrawGizmos()
        {
            GizmoTools.DrawSphereCast(_lastHit, GetCenter(), _overridedMaxDistanceFunc?.Invoke() ?? 0.1f, Vector3.down,
                GetRadius(), Color.red);
        }

        public RaycastHit GetGroundHitUnderFeet(out bool changedHitCollider)
        {
            if (_lastCheckFrame == Time.frameCount)
            {
                changedHitCollider = _changedHitCollider;
                return _lastHit;
            }

            var hitsCount = UnityEngine.Physics.SphereCastNonAlloc(GetCenter(), GetRadius(),
                Vector3.down,
                _results,
                _overridedMaxDistanceFunc?.Invoke() ?? 0.1f,
                _physicsLayersConfig.LayerMaskByLayerGroup[PhysicsLayerGroup.Walkable],
                QueryTriggerInteraction.Ignore);

            var lastCollider = _lastHit.collider;

            _lastHit = _raycastHitFilter.Invoke(_results, hitsCount);
            _lastCheckFrame = Time.frameCount;

            _changedHitCollider = changedHitCollider = _lastHit.collider == lastCollider;

            return _lastHit;
        }

        private RaycastHit GetFirstRaycastHit(RaycastHit[] hits, int hitsCount)
        {
            return hitsCount > 0 ? hits.FirstOrDefault() : default;
        }
    }
}