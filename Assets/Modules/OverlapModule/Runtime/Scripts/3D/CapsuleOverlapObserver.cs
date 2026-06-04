using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts._3D
{
    public class CapsuleOverlapObserver : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector3 LocalPoint0 { get; private set; } = Vector3.zero;

            [field: SerializeField]
            public Vector3 LocalPoint1 { get; private set; } = new Vector3(0.5f, 0.5f, 0.5f);

            [field: SerializeField]
            [field: Range(0.01f, 10f)] public float Radius { get; private set; } = 0.5f;
            
            public Vector3 GetLocalPoint0() => LocalPoint0;
            public Vector3 GetLocalPoint1() => LocalPoint1;
            public float GetRadius() => Radius;
        }
        
        public class Model
        {
            public ValueModel<Vector3> LocalPoint0Model { get; }
            public ValueModel<Vector3> LocalPoint1Model { get; }
            public ValueModel<float> RadiusModel { get; }

            public Model(Config config)
            {
                LocalPoint0Model = new ValueModel<Vector3>(config.GetLocalPoint0);
                LocalPoint1Model = new ValueModel<Vector3>(config.GetLocalPoint1);
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

        protected override int GetAddedOverlapResultsNumber(Collider[] overlapResultsBuffer)
        {
            var worldPoint0 = transform.TransformPoint(LocalModel.LocalPoint0Model.Value);
            var worldPoint1 = transform.TransformPoint(LocalModel.LocalPoint1Model.Value);

            return Physics.OverlapCapsuleNonAlloc(worldPoint0, worldPoint1, LocalModel.RadiusModel.Value, overlapResultsBuffer,
                SharedModel.LayerMaskModel.Value, SharedModel.QueryTriggerInteractionModel.Value);
        }

        protected override void TryMakeNewOverlapAdded(int numberOverlaps, List<Collider> addedOverlaps,
            Collider[] overlapResultsBuffer, HashSet<Collider> currentOverlaps,
            HashSet<Collider> previouslyOverlaps)
        {
            for (int i = 0; i < numberOverlaps; i++)
            {
                Collider overlap = overlapResultsBuffer[i];

                if (currentOverlaps.Add(overlap))
                {
                    if (!previouslyOverlaps.Contains(overlap))
                    {
                        addedOverlaps.Add(overlap);
                    }
                }
            }
        }

        protected override void DrawGizmos(bool selected)
        {
            var worldPoint0 = transform.TransformPoint(LocalModel.LocalPoint0Model.Value);
            var worldPoint1 = transform.TransformPoint(LocalModel.LocalPoint1Model.Value);
            
            Gizmos.DrawWireSphere(worldPoint0, LocalModel.RadiusModel.Value);
            Gizmos.DrawWireSphere(worldPoint1, LocalModel.RadiusModel.Value);

            var direction = (worldPoint1 - worldPoint0).normalized;
            
            var rotation = Quaternion.LookRotation(direction);
            var side1 = rotation * Vector3.right * LocalModel.RadiusModel.Value;
            var side2 = rotation * Vector3.up * LocalModel.RadiusModel.Value;
            
            Gizmos.DrawLine(worldPoint0 + side1, worldPoint1 + side1);
            Gizmos.DrawLine(worldPoint0 - side1, worldPoint1 - side1);
            Gizmos.DrawLine(worldPoint0 + side2, worldPoint1 + side2);
            Gizmos.DrawLine(worldPoint0 - side2, worldPoint1 - side2);
        }
    }
}