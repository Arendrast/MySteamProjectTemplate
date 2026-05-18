using System;
using System.Collections.Generic;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public class BoxOverlapObserver : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector3 LocalCenter { get; private set; } = Vector3.zero;

            [field: SerializeField]
            public Vector3 LocalHalfExtents { get; private set; } = new Vector3(0.5f, 0.5f, 0.5f);

            [field: SerializeField] public Quaternion LocalRotation { get; private set; } = Quaternion.identity;
        }
        
        [SerializeField] private Config _config;

        public Vector3 GetCheckCenter() => transform.TransformPoint(_config.LocalCenter);


        protected override int GetAddedOverlapResultsNumber(Collider[] overlapResultsBuffer)
        {
            Vector3 worldCenter = GetCheckCenter();

            Quaternion worldRotation = transform.rotation * _config.LocalRotation;

            return Physics.OverlapBoxNonAlloc(
                worldCenter,
                _config.LocalHalfExtents.Multiply(transform.lossyScale)
                    .Abs(),
                overlapResultsBuffer,
                worldRotation,
                OverlapConfig.LayerMask,
                OverlapConfig.QueryTriggerInteraction);
        }

        protected override void DrawGizmos(bool selected)
        {
            Matrix4x4 currentGizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            Matrix4x4 boxLocalMatrix = Matrix4x4.TRS(_config.LocalCenter, _config.LocalRotation, Vector3.one);
            Gizmos.matrix *= boxLocalMatrix;

            if (selected)
            {
                Gizmos.DrawWireCube(Vector3.zero, _config.LocalHalfExtents * 2);
            }
            else
            {
                Gizmos.DrawCube(Vector3.zero, _config.LocalHalfExtents * 2);
            }

            Gizmos.matrix = currentGizmoMatrix;
        }
    }
}