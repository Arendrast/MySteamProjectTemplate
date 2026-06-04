using System;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts._3D
{
    public class CylinderOverlapObserver : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector3 LocalPosition { get; private set; } = Vector3.zero;
            [field: SerializeField] public Quaternion LocalRotation { get; private set; } = Quaternion.identity;

            [field: SerializeField]
            [field: Min(0.01f)]
            public float Radius { get; private set; } = 0.5f;

            [field: SerializeField]
            [field: Min(0.01f)]
            public float Height { get; private set; } = 1f;

            public Vector3 GetLocalPosition() => LocalPosition;
            public Quaternion GetLocalRotation() => LocalRotation;
            public float GetRadius() => Radius;
            public float GetHeight() => Height;
        }

        public class Model
        {
            public ValueModel<Vector3> LocalPositionModel { get; }
            public ValueModel<Quaternion> LocalRotationModel { get; }
            public ValueModel<float> RadiusModel { get; }
            public ValueModel<float> HeightModel { get; }

            public Model(Config config)
            {
                LocalPositionModel = new ValueModel<Vector3>(config.GetLocalPosition);
                LocalRotationModel = new ValueModel<Quaternion>(config.GetLocalRotation);
                RadiusModel = new ValueModel<float>(config.GetRadius);
                HeightModel = new ValueModel<float>(config.GetHeight);
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
            Vector3 worldCenter = transform.TransformPoint(LocalModel.LocalPositionModel.Value);
            Quaternion worldRotation = transform.rotation * LocalModel.LocalRotationModel.Value;
            Vector3 worldUpDirection = worldRotation * Vector3.up;

            float halfHeight = LocalModel.HeightModel.Value / 2f;

            Vector3 capsulePoint1 = worldCenter + worldUpDirection * (halfHeight - LocalModel.RadiusModel.Value);
            Vector3 capsulePoint2 = worldCenter - worldUpDirection * (halfHeight - LocalModel.RadiusModel.Value);

            if (LocalModel.HeightModel.Value <= 2f * LocalModel.RadiusModel.Value)
            {
                capsulePoint1 = worldCenter;
                capsulePoint2 = worldCenter;
            }

            return Physics.OverlapCapsuleNonAlloc(
                capsulePoint1,
                capsulePoint2,
                LocalModel.RadiusModel.Value,
                overlapResultsBuffer,
                SharedModel.LayerMaskModel.Value,
                SharedModel.QueryTriggerInteractionModel.Value
            );
        }

        protected override void DrawGizmos(bool selected)
        {
            Matrix4x4 currentGizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            Matrix4x4 cylinderLocalMatrix = Matrix4x4.TRS(LocalModel.LocalPositionModel.Value, LocalModel.LocalRotationModel.Value, Vector3.one);
            Gizmos.matrix *= cylinderLocalMatrix;

            DrawWireCylinder(LocalModel.RadiusModel.Value, LocalModel.HeightModel.Value);

            Gizmos.matrix = currentGizmoMatrix;
        }

        private void DrawWireCylinder(float radius, float height, int segments = 20)
        {
            var topCenter = Vector3.up * (height / 2f);
            var bottomCenter = Vector3.down * (height / 2f);

            DrawWireCircle(topCenter, Quaternion.identity, radius, segments);
            DrawWireCircle(bottomCenter, Quaternion.identity, radius, segments);

            for (int i = 0; i < segments; i++)
            {
                var angle = (float)i / segments * Mathf.PI * 2f;
                var offset = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(topCenter + offset, bottomCenter + offset);
                Popcron.Gizmos.Line(topCenter + offset, bottomCenter + offset, Gizmos.color);
            }
        }

        private void DrawWireCircle(Vector3 center, Quaternion rotation, float radius, int segments)
        {
            var prevPoint = center + rotation * new Vector3(radius, 0, 0);
            for (int i = 1; i <= segments; i++)
            {
                var angle = (float)i / segments * Mathf.PI * 2f;
                var currentPoint =
                    center + rotation * new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(prevPoint, currentPoint);
                Popcron.Gizmos.Line(prevPoint, currentPoint, Gizmos.color);
                prevPoint = currentPoint;
            }
        }
    }
}