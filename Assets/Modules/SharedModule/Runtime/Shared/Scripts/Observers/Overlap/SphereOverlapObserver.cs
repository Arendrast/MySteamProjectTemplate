using System;
using System.Collections.Generic;
using MoreLinq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public class SphereOverlapObserver : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector3 LocalCenter { get; private set; } = Vector3.zero;
           
            [field: SerializeField]
            [field: Min(0.01f)]
            public float Radius { get; private set; } = 5f;
        }

        [SerializeField] private Config _config;

        public Vector3 GetCheckCenter() => transform.TransformPoint(_config.LocalCenter);

        protected override int GetAddedOverlapResultsNumber(Collider[] overlapResultsBuffer)
        {
            return Physics.OverlapSphereNonAlloc(
                GetCheckCenter(),
                _config.Radius,
                overlapResultsBuffer,
                OverlapConfig.LayerMask,
                OverlapConfig.QueryTriggerInteraction);
        }

        protected override void DrawGizmos(bool selected)
        {
            Matrix4x4 currentGizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            Matrix4x4 sphereLocalMatrix = Matrix4x4.TRS(_config.LocalCenter, Quaternion.identity, Vector3.one);
            Gizmos.matrix *= sphereLocalMatrix;
            
            if (selected)
            {
                Gizmos.DrawWireSphere(center: Vector3.zero, radius: _config.Radius);
            }
            else
            {
                Gizmos.DrawSphere(center: Vector3.zero, radius: _config.Radius);
            }
            
            Gizmos.matrix = currentGizmoMatrix;
        }
    }
}