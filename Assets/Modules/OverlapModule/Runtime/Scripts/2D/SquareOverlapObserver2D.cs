using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts._2D
{
    public class SquareOverlapObserver : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector2 LocalPosition { get; private set; } = Vector2.zero;
            [field: SerializeField] public Vector2 Size { get; private set; } = new Vector2(1, 1);
            [field: SerializeField] public float Angle { get; private set; } = 0f;

            public Vector2 GetLocalPosition() => LocalPosition;
            public float GetAngle() => Angle;
            public Vector2 GetLocalHalfExtents() => Size;
        }

        public class Model
        {
            public ValueModel<Vector2> LocalPositionModel { get; }
            public ValueModel<Vector2> SizeModel { get; }
            public ValueModel<float> AngleModel { get; }

            public Model(Config config)
            {
                LocalPositionModel = new ValueModel<Vector2>(config.GetLocalPosition);
                SizeModel = new ValueModel<Vector2>(config.GetLocalHalfExtents);
                AngleModel = new ValueModel<float>(config.GetAngle);
            }
        }

        public Model LocalModel { get; private set; }

        [SerializeField] private Config _config;

        protected override void Awake()
        {
            base.Awake();
            LocalModel = new Model(_config);
        }

        public Vector2 GetCheckCenter() => transform.TransformPoint(LocalModel.LocalPositionModel.Value);

        protected override int GetAddedOverlapResultsNumber(Collider2D[] overlapResultsBuffer)
        {
            Vector2 worldCenter = GetCheckCenter();

            return Physics2D.OverlapBox(
                worldCenter,
                LocalModel.SizeModel.Value.Multiply(transform.lossyScale).Abs() * 2f,
                LocalModel.AngleModel.Value,
                SharedModel.ContactFilterModel.Value, overlapResultsBuffer);
        }

        protected override void DrawGizmos(bool selected)
        {
            var currentGizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            var boxLocalMatrix = Matrix4x4.TRS(LocalModel.LocalPositionModel.Value,
                Quaternion.Euler(0, 0, LocalModel.AngleModel.Value), Vector3.one);

            Gizmos.matrix *= boxLocalMatrix;
            Gizmos.DrawWireCube(Vector3.zero, LocalModel.SizeModel.Value * 2f);
            Gizmos.matrix = currentGizmoMatrix;
        }
    }
}