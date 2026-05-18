using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
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
            
            public void SetLocalPoints(Vector3 localPoint0, Vector3 localPoint1)
            {
                LocalPoint0 = localPoint0;
                LocalPoint1 = localPoint1;
            }
        }
        
        private float Radius => Mathf.Abs(_config.Radius);
            
        [SerializeField] private Config _config;

        private Vector3 _defaultLocalPoint0, _defaultLocalPoint1;
        
        
        public void SetLocalPoints(Vector3 localPoint0, Vector3 localPoint1) => _config.SetLocalPoints(localPoint0, localPoint1);
        public void SetLocalPointsToDefault() => SetLocalPoints(_defaultLocalPoint0, _defaultLocalPoint1);

        protected override int GetAddedOverlapResultsNumber(Collider[] overlapResultsBuffer)
        {
            var worldPoint0 = transform.TransformPoint(_config.LocalPoint0);
            var worldPoint1 = transform.TransformPoint(_config.LocalPoint1);

            return Physics.OverlapCapsuleNonAlloc(worldPoint0, worldPoint1, Radius, overlapResultsBuffer,
                OverlapConfig.LayerMask, OverlapConfig.QueryTriggerInteraction);
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
            Vector3 worldPoint0 = transform.TransformPoint(_config.LocalPoint0);
            Vector3 worldPoint1 = transform.TransformPoint(_config.LocalPoint1);
            
            Gizmos.DrawWireSphere(worldPoint0, Radius);
            Gizmos.DrawWireSphere(worldPoint1, Radius);

            Vector3 direction = (worldPoint1 - worldPoint0).normalized;
            
            Quaternion rotation = Quaternion.LookRotation(direction);
            Vector3 side1 = rotation * Vector3.right * Radius;
            Vector3 side2 = rotation * Vector3.up * Radius;
            
            Gizmos.DrawLine(worldPoint0 + side1, worldPoint1 + side1);
            Gizmos.DrawLine(worldPoint0 - side1, worldPoint1 - side1);
            Gizmos.DrawLine(worldPoint0 + side2, worldPoint1 + side2);
            Gizmos.DrawLine(worldPoint0 - side2, worldPoint1 - side2);
        }
    }
}