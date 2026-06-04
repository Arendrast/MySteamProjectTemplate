using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts._2D
{
    public class CircleOverlapObserver2D : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector2 LocalPosition { get; private set; } = Vector3.zero;
           
            [field: SerializeField]
            [field: Min(0.01f)]
            public float Radius { get; private set; } = 5f;
            
            public Vector2 GetLocalPosition() => LocalPosition;
            public float GetRadius() => Radius;
        }
        
        public class Model
        {
            public ValueModel<Vector2> LocalPositionModel { get; }
            public ValueModel<float> RadiusModel { get; }
            
            public Model(Config config)
            {
                LocalPositionModel = new ValueModel<Vector2>(config.GetLocalPosition);
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
        
        public Vector3 GetCheckCenter() => transform.TransformPoint(LocalModel.LocalPositionModel.Value);

        protected override int GetAddedOverlapResultsNumber(Collider2D[] overlapResultsBuffer)
        {
            return Physics2D.OverlapCircle(
                GetCheckCenter(),
                LocalModel.RadiusModel.Value,
                SharedModel.ContactFilterModel.Value,
                overlapResultsBuffer);
        }

        protected override void DrawGizmos(bool selected)
        {
            GizmoTools.DrawWireCircle2D(LocalModel.LocalPositionModel.Value, LocalModel.RadiusModel.Value);
        }
    }
}