using UnityEngine;

namespace ProjectName.DebugModule.Shared.Scripts
{
    public class SphereGizmoDrawer : GizmoDrawerBase
    {
        [Header("Настройки сферы")] [Tooltip("Радиус сферы.")] [Min(0.001f)]
        // Гарантируем, что радиус будет положительным
        public float radius = 0.5f; // По умолчанию радиус 0.5

        // Переопределяем метод для отрисовки конкретной формы - сферы
        protected override void DrawGizmoShape(Vector3 center)
        {
            // Отрисовываем закрашенную сферу
            Gizmos.DrawSphere(center, radius);
            // Опционально: отрисовываем каркасную сферу для лучшей видимости границ
            Gizmos.DrawWireSphere(center, radius);
        }
    }
}