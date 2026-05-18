using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public class CylinderOverlapObserver : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector3 LocalCenter { get; private set; } = Vector3.zero;
            [field: SerializeField] public Quaternion LocalRotation { get; private set; } = Quaternion.identity;

            [field: SerializeField]
            [field: Min(0.01f)]
            public float Radius { get; private set; } = 0.5f;

            [field: SerializeField]
            [field: Min(0.01f)]
            public float Height { get; private set; } = 1f;
        }

        private float Radius => Mathf.Abs(_config.Radius);
        private float Height => Mathf.Abs(_config.Height);

        [SerializeField] private Config _config;

        public Vector3 GetCheckCenter() => transform.TransformPoint(_config.LocalCenter);

        protected override int GetAddedOverlapResultsNumber(Collider[] overlapResultsBuffer)
        {
            Vector3 worldCenter = transform.TransformPoint(_config.LocalCenter);
            Quaternion worldRotation = transform.rotation * _config.LocalRotation;
            Vector3 worldUpDirection = worldRotation * Vector3.up;

            float halfHeight = Height / 2f;

            Vector3 capsulePoint1 = worldCenter + worldUpDirection * (halfHeight - Radius);
            Vector3 capsulePoint2 = worldCenter - worldUpDirection * (halfHeight - Radius);

            if (Height <= 2f * Radius)
            {
                capsulePoint1 = worldCenter;
                capsulePoint2 = worldCenter;
            }

            return Physics.OverlapCapsuleNonAlloc(
                capsulePoint1,
                capsulePoint2,
                Radius,
                overlapResultsBuffer,
                OverlapConfig.LayerMask,
                OverlapConfig.QueryTriggerInteraction
            );
        }

        protected override void DrawGizmos(bool selected)
        {
            Matrix4x4 currentGizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            // Применяем локальное смещение и вращение цилиндра
            Matrix4x4 cylinderLocalMatrix = Matrix4x4.TRS(_config.LocalCenter, _config.LocalRotation, Vector3.one);
            Gizmos.matrix *= cylinderLocalMatrix;

            DrawWireCylinder(Radius, Height);

            Gizmos.matrix = currentGizmoMatrix;
        }

        private void DrawWireCylinder(float radius, float height, int segments = 20)
        {
            Vector3 topCenter = Vector3.up * (height / 2f);
            Vector3 bottomCenter = Vector3.down * (height / 2f);

            DrawWireCircle(topCenter, Quaternion.identity, radius, segments);
            DrawWireCircle(bottomCenter, Quaternion.identity, radius, segments);

            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(topCenter + offset, bottomCenter + offset);
                Popcron.Gizmos.Line(topCenter + offset, bottomCenter + offset, Gizmos.color);
            }
        }

        private void DrawWireCircle(Vector3 center, Quaternion rotation, float radius, int segments)
        {
            Vector3 prevPoint = center + rotation * new Vector3(radius, 0, 0);
            for (int i = 1; i <= segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                Vector3 currentPoint =
                    center + rotation * new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(prevPoint, currentPoint);
                Popcron.Gizmos.Line(prevPoint, currentPoint, Gizmos.color);
                prevPoint = currentPoint;
            }
        }
    }
}