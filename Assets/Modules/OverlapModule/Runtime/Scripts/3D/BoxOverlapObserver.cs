using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts._3D
{
    public class BoxOverlapObserver : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector3 LocalPosition { get; private set; } = Vector3.zero;
            [field: SerializeField] public Vector3 LocalHalfExtents { get; private set; } = new Vector3(0.5f, 0.5f, 0.5f);
            [field: SerializeField] public Quaternion LocalRotation { get; private set; } = Quaternion.identity;
            
            public Vector3 GetLocalPosition() => LocalPosition;
            public Quaternion GetLocalRotation() => LocalRotation;
            public Vector3 GetLocalHalfExtents() => LocalHalfExtents;
        }

        public class Model
        {
            public ValueModel<Vector3> LocalPositionModel { get; }
            public ValueModel<Vector3> LocalHalfExtentsModel { get; }
            public ValueModel<Quaternion> LocalRotationModel { get; }

            public Model(Config config)
            {
                LocalPositionModel = new ValueModel<Vector3>(config.GetLocalPosition);
                LocalHalfExtentsModel = new ValueModel<Vector3>(config.GetLocalHalfExtents);
                LocalRotationModel = new ValueModel<Quaternion>(config.GetLocalRotation);
            }
        }
        
        public Model LocalModel { get; private set; }
        
        [SerializeField] private Config _config = new Config();

        protected override void Awake()
        {
            base.Awake();
            LocalModel = new Model(_config);
        }

        public Vector3 GetCheckCenter() => transform.TransformPoint(LocalModel.LocalPositionModel.Value);

        protected override int GetAddedOverlapResultsNumber(Collider[] overlapResultsBuffer)
        {
            Vector3 worldCenter = GetCheckCenter();

            Quaternion worldRotation = transform.rotation * LocalModel.LocalRotationModel.Value;

            return Physics.OverlapBoxNonAlloc(
                worldCenter,
                LocalModel.LocalHalfExtentsModel.Value.Multiply(transform.lossyScale)
                    .Abs(),
                overlapResultsBuffer,
                worldRotation,
                SharedModel.LayerMaskModel.Value,
                SharedModel.QueryTriggerInteractionModel.Value);
        }

        protected override void DrawGizmos(bool selected)
        {
            var currentGizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            var boxLocalMatrix = Matrix4x4.TRS(LocalModel.LocalPositionModel.Value, LocalModel.LocalRotationModel.Value, Vector3.one);
           
            Gizmos.matrix *= boxLocalMatrix;
            Gizmos.DrawWireCube(Vector3.zero, LocalModel.LocalHalfExtentsModel.Value * 2);
            Gizmos.matrix = currentGizmoMatrix;
        }
    }
}