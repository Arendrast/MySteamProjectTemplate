using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts._2D
{
    public class CapsuleOverlapObserver2D : OverlapObserver
    {
        [Serializable]
        public class Config
        {
            [field: SerializeField] public Vector2 LocalPosition { get; private set; } = Vector2.zero;
            [field: SerializeField] public Vector2 Size { get; private set; } = new Vector2(1, 1);

            [field: SerializeField]
            public CapsuleDirection2D CapsuleDirection2D { get; private set; } = CapsuleDirection2D.Vertical;

            [field: SerializeField] public float Angle { get; private set; }

            [field: SerializeField]
            [field: Range(0.01f, 10f)]
            public float Radius { get; private set; } = 0.5f;
            
            public Vector2 GetLocalPosition() => LocalPosition;
            public Vector2 GetSize() => Size;
            public CapsuleDirection2D GetCapsuleDirection2D() => CapsuleDirection2D;
            public float GetAngle() => Angle;
            public float GetRadius() => Radius;
        }

        public class Model
        {
            public ValueModel<Vector2> LocalPositionModel { get; }
            public ValueModel<Vector2> SizeModel { get; }
            public ValueModel<CapsuleDirection2D> CapsuleDirection2DModel { get; }
            public ValueModel<float> AngleModel { get; }
            public ValueModel<float> RadiusModel { get; }
            
            public Model(Config config)
            {
                LocalPositionModel = new ValueModel<Vector2>(config.GetLocalPosition);
                SizeModel = new ValueModel<Vector2>(config.GetSize);
                CapsuleDirection2DModel = new ValueModel<CapsuleDirection2D>(config.GetCapsuleDirection2D);
                AngleModel = new ValueModel<float>(config.GetAngle);
                RadiusModel = new ValueModel<float>(config.GetRadius);
            }
        }
        
        public Model LocalModel { get; private set; }
        
        [SerializeField] private Config _config;
        
        private CapsuleOverlapObserver2DGizmosDrawer _gizmosDrawer;

        protected override void Awake()
        {
            base.Awake();
            LocalModel = new Model(_config);
            _gizmosDrawer = new CapsuleOverlapObserver2DGizmosDrawer(transform, LocalModel);
        }

        protected override int GetAddedOverlapResultsNumber(Collider2D[] overlapResultsBuffer)
        {
            var position = transform.TransformPoint(LocalModel.LocalPositionModel.Value);

            return Physics2D.OverlapCapsule(position, LocalModel.SizeModel.Value, LocalModel.CapsuleDirection2DModel.Value, LocalModel.AngleModel.Value,
                SharedModel.ContactFilterModel.Value,
                overlapResultsBuffer);
        }

        protected override void TryMakeNewOverlapAdded(int numberOverlaps, List<Collider2D> addedOverlaps,
            Collider2D[] overlapResultsBuffer, HashSet<Collider2D> currentOverlaps,
            HashSet<Collider2D> previouslyOverlaps)
        {
            for (int i = 0; i < numberOverlaps; i++)
            {
                Collider2D overlap = overlapResultsBuffer[i];

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
            _gizmosDrawer.DrawGizmos(selected);
        }
    }
}