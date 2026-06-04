using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts._3D
{
    public class ConOverlapObserver : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector3 LocalPosition { get; private set; } = Vector3.zero;

            [field: SerializeField] public Vector3 LocalDirection { get; private set; } = Vector3.forward;

            [field: SerializeField]
            [field: Range(0, 360f)]
            public float Angle { get; private set; } = 45f;

            [field: SerializeField]
            [field: Min(0.01f)]
            public float Range { get; private set; } = 5f;
            
            public Vector3 GetLocalPosition() => LocalPosition;
            public Vector3 GetLocalDirection() => LocalDirection;
            public float GetAngle() => Angle;
            public float GetRange() => Range;
        }

        public class Model
        {
            public ValueModel<Vector3> LocalPositionModel { get; }
            public ValueModel<Vector3> LocalDirectionModel { get; }
            public ValueModel<float> AngleModel { get; }
            public ValueModel<float> RangeModel { get; }

            public Model(Config config)
            {
                LocalPositionModel = new ValueModel<Vector3>(config.GetLocalPosition);
                LocalDirectionModel = new ValueModel<Vector3>(config.GetLocalDirection);
                AngleModel = new ValueModel<float>(config.GetAngle);
                RangeModel = new ValueModel<float>(config.GetRange);
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
                LocalModel.RangeModel.Value,
                overlapResultsBuffer,
                SharedModel.LayerMaskModel.Value,
                SharedModel.QueryTriggerInteractionModel.Value);
        }

        protected override void TryMakeNewOverlapAdded(int numberOverlaps, List<Collider> addedOverlaps,
            Collider[] overlapResultsBuffer, HashSet<Collider> currentOverlaps,
            HashSet<Collider> previouslyOverlaps)
        {
            var worldDirection = transform.TransformDirection(LocalModel.LocalDirectionModel.Value).normalized;
            var worldCenter = GetCheckCenter();

            for (int i = 0; i < numberOverlaps; i++)
            {
                var detectedCollider = overlapResultsBuffer[i];

                var directionToTarget = (detectedCollider.bounds.center - worldCenter).normalized;

                if (directionToTarget == Vector3.zero) directionToTarget = worldDirection;

                var angleToTarget = Vector3.Angle(worldDirection, directionToTarget);

                if (angleToTarget <= LocalModel.AngleModel.Value * 0.5f)
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
            var center = transform.TransformPoint(LocalModel.LocalPositionModel.Value);
            var direction = transform.TransformDirection(LocalModel.LocalDirectionModel.Value).normalized;

            Gizmos.DrawRay(center, direction * LocalModel.RangeModel.Value);

            var rad = LocalModel.AngleModel.Value * 0.5f * Mathf.Deg2Rad;
            var coneRadius = Mathf.Tan(rad) * LocalModel.RangeModel.Value;

            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(center + direction * LocalModel.RangeModel.Value, Quaternion.LookRotation(direction),
                Vector3.one);

            DrawWireDisk(coneRadius);

            Gizmos.matrix = oldMatrix;

            var rotation = Quaternion.LookRotation(direction);
            var up = rotation * Vector3.up * coneRadius;
            var right = rotation * Vector3.right * coneRadius;
            var baseCenter = center + direction * LocalModel.RangeModel.Value;

            Gizmos.DrawLine(center, baseCenter + up);
            Gizmos.DrawLine(center, baseCenter - up);
            Gizmos.DrawLine(center, baseCenter + right);
            Gizmos.DrawLine(center, baseCenter - right);
        }

        private void DrawWireDisk(float radius)
        {
            float step = 20f;
            var prev = new Vector3(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius, 0);
            for (float a = step; a <= 360f; a += step)
            {
                var next = new Vector3(Mathf.Cos(a * Mathf.Deg2Rad) * radius, Mathf.Sin(a * Mathf.Deg2Rad) * radius, 0);
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
        }
    }
}