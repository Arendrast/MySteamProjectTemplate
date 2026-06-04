using System;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts._3D
{
    public class SphereOverlapObserver : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector3 LocalPosition { get; private set; } = Vector3.zero;

            [field: SerializeField]
            [field: Min(0.01f)]
            public float Radius { get; private set; } = 5f;

            public Vector3 GetLocalPosition() => LocalPosition;
            public float GetRadius() => Radius;
        }

        public class Model
        {
            public ValueModel<Vector3> LocalPositionModel { get; }
            public ValueModel<float> RadiusModel { get; }

            public Model(Config config)
            {
                LocalPositionModel = new ValueModel<Vector3>(config.GetLocalPosition);
                RadiusModel = new ValueModel<float>(config.GetRadius);
            }
        }

        public Model LocalModel { get; private set; }

        [SerializeField] private Config _config;

        protected override void Awake()
        {
            base.Awake();
            LocalModel = new Model(_config);
        }

        public Vector3 GetCheckCenter() => transform.TransformPoint(LocalModel.LocalPositionModel.Value);

        protected override int GetAddedOverlapResultsNumber(Collider[] overlapResultsBuffer)
        {
            return Physics.OverlapSphereNonAlloc(
                GetCheckCenter(),
                LocalModel.RadiusModel.Value,
                overlapResultsBuffer,
                SharedModel.LayerMaskModel.Value,
                SharedModel.QueryTriggerInteractionModel.Value);
        }

        protected override void DrawGizmos(bool selected)
        {
            var currentGizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            var sphereLocalMatrix =
                Matrix4x4.TRS(LocalModel.LocalPositionModel.Value, Quaternion.identity, Vector3.one);
            Gizmos.matrix *= sphereLocalMatrix;

            Gizmos.DrawWireSphere(center: Vector3.zero, radius: LocalModel.RadiusModel.Value);

            Gizmos.matrix = currentGizmoMatrix;
        }
    }
}