using System;
using System.Collections.Generic;
using MoreLinq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public class ConOverlapObserver : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector3 LocalCenter { get; private set; } = Vector3.zero;

            [field: SerializeField] public Vector3 LocalDirection { get; private set; } = Vector3.forward;

            [field: SerializeField]
            [field: Range(0, 360f)]
            public float Angle { get; private set; } = 45f;

            [field: SerializeField]
            [field: Min(0.01f)]
            public float Range { get; private set; } = 5f;
        }

        [SerializeField] private Config _config;

        public Vector3 GetCheckCenter() => transform.TransformPoint(_config.LocalCenter);

        protected override int GetAddedOverlapResultsNumber(Collider[] overlapResultsBuffer)
        {
            return Physics.OverlapSphereNonAlloc(
                GetCheckCenter(),
                _config.Range,
                overlapResultsBuffer,
                OverlapConfig.LayerMask,
                OverlapConfig.QueryTriggerInteraction);
        }

        protected override void TryMakeNewOverlapAdded(int numberOverlaps, List<Collider> addedOverlaps,
            Collider[] overlapResultsBuffer, HashSet<Collider> currentOverlaps,
            HashSet<Collider> previouslyOverlaps)
        {
            var worldDirection = transform.TransformDirection(_config.LocalDirection).normalized;
            var worldCenter = GetCheckCenter();

            for (int i = 0; i < numberOverlaps; i++)
            {
                var detectedCollider = overlapResultsBuffer[i];

                var directionToTarget = (detectedCollider.bounds.center - worldCenter).normalized;

                if (directionToTarget == Vector3.zero) directionToTarget = worldDirection;

                var angleToTarget = Vector3.Angle(worldDirection, directionToTarget);

                if (angleToTarget <= _config.Angle * 0.5f)
                {
                    if (currentOverlaps.Add(detectedCollider))
                    {
                        if (!previouslyOverlaps.Contains(detectedCollider))
                        {
                            addedOverlaps.Add(detectedCollider);
                        }
                    }
                }
            }
        }

        protected override void DrawGizmos(bool selected)
        {
            Vector3 center = transform.TransformPoint(_config.LocalCenter);
            Vector3 direction = transform.TransformDirection(_config.LocalDirection).normalized;

            Gizmos.DrawRay(center, direction * _config.Range);

            float rad = _config.Angle * 0.5f * Mathf.Deg2Rad;
            float coneRadius = Mathf.Tan(rad) * _config.Range;

            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(center + direction * _config.Range, Quaternion.LookRotation(direction),
                Vector3.one);

            DrawWireDisk(coneRadius);

            Gizmos.matrix = oldMatrix;

            Quaternion rot = Quaternion.LookRotation(direction);
            Vector3 up = rot * Vector3.up * coneRadius;
            Vector3 right = rot * Vector3.right * coneRadius;
            Vector3 baseCenter = center + direction * _config.Range;

            Gizmos.DrawLine(center, baseCenter + up);
            Gizmos.DrawLine(center, baseCenter - up);
            Gizmos.DrawLine(center, baseCenter + right);
            Gizmos.DrawLine(center, baseCenter - right);
        }

        private void DrawWireDisk(float radius)
        {
            float step = 20f;
            Vector3 prev = new Vector3(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius, 0);
            for (float a = step; a <= 360f; a += step)
            {
                Vector3 next = new Vector3(Mathf.Cos(a * Mathf.Deg2Rad) * radius, Mathf.Sin(a * Mathf.Deg2Rad) * radius,
                    0);
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
        }
    }
}