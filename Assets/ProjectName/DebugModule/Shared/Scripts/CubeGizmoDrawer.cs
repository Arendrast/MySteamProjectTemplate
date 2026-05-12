using UnityEngine;

namespace ProjectName.DebugModule.Shared.Scripts
{
    public class CubeGizmoDrawer : GizmoDrawerBase
    {
        [Header("Настройки куба")] [Tooltip("Размер куба по осям X, Y, Z.")] [Min(0.001f)]
        // Гарантируем, что размер будет положительным
        public Vector3 size = Vector3.one; // По умолчанию 1x1x1

        // Переопределяем метод для отрисовки конкретной формы - куба
        protected override void DrawGizmoShape(Vector3 center)
        {
            // Отрисовываем закрашенный куб
            Gizmos.DrawCube(center, size);
            // Опционально: отрисовываем каркасный куб для лучшей видимости границ
            Gizmos.DrawWireCube(center, size);
        }
    }
}