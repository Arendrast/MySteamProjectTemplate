using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts._2D
{
    public class CapsuleOverlapObserver2DGizmosDrawer
    {
        private readonly Transform _transform;
        private readonly CapsuleOverlapObserver2D.Model _model;

        public CapsuleOverlapObserver2DGizmosDrawer(Transform transform, CapsuleOverlapObserver2D.Model model)
        {
            _transform = transform;
            _model = model;
        }

        public void DrawGizmos(bool selected)
        {
            var currentGizmoMatrix = Gizmos.matrix;
            
            var capsuleRotation = Quaternion.Euler(0f, 0f, _model.AngleModel.Value);
            var capsuleLocalMatrix = Matrix4x4.TRS(_model.LocalPositionModel.Value, capsuleRotation, Vector3.one);
            
            Gizmos.matrix = _transform.localToWorldMatrix * capsuleLocalMatrix;
            
            DrawLocalCapsule2D(_model.SizeModel.Value, _model.CapsuleDirection2DModel.Value);
            
            Gizmos.matrix = currentGizmoMatrix;
        }

        private void DrawLocalCapsule2D(Vector2 size, CapsuleDirection2D direction)
        {
            var width = Mathf.Max(0.01f, size.x);
            var height = Mathf.Max(0.01f, size.y);

            if (direction == CapsuleDirection2D.Vertical)
            {
                var radius = width / 2f;
                var halfHeight = height / 2f;

                if (halfHeight <= radius)
                {
                    GizmoTools.DrawWireCircle2D(Vector3.zero, radius);
                    return;
                }

                var straightHalfHeight = halfHeight - radius;

                var topCenter = new Vector3(0f, straightHalfHeight, 0f);
                var bottomCenter = new Vector3(0f, -straightHalfHeight, 0f);

                Gizmos.DrawLine(new Vector3(-radius, straightHalfHeight, 0f),
                    new Vector3(-radius, -straightHalfHeight, 0f));
                Gizmos.DrawLine(new Vector3(radius, straightHalfHeight, 0f),
                    new Vector3(radius, -straightHalfHeight, 0f));

                GizmoTools.DrawArc2D(topCenter, 0f, Mathf.PI, radius);

                GizmoTools.DrawArc2D(bottomCenter, Mathf.PI, Mathf.PI * 2f, radius);
            }
            else
            {
                var radius = height / 2f;
                var halfWidth = width / 2f;

                if (halfWidth <= radius)
                {
                    GizmoTools.DrawWireCircle2D(Vector3.zero, radius);
                    return;
                }

                var straightHalfWidth = halfWidth - radius;

                var rightCenter = new Vector3(straightHalfWidth, 0f, 0f);
                var leftCenter = new Vector3(-straightHalfWidth, 0f, 0f);

                Gizmos.DrawLine(new Vector3(-straightHalfWidth, radius, 0f),
                    new Vector3(straightHalfWidth, radius, 0f));
                Gizmos.DrawLine(new Vector3(-straightHalfWidth, -radius, 0f),
                    new Vector3(straightHalfWidth, -radius, 0f));

                GizmoTools.DrawArc2D(rightCenter, -Mathf.PI / 2f, Mathf.PI / 2f, radius);

                GizmoTools.DrawArc2D(leftCenter, Mathf.PI / 2f, Mathf.PI * 1.5f, radius);
            }
        }
    }
}