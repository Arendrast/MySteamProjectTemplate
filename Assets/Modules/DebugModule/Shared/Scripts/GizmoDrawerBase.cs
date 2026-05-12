using UnityEngine;

namespace Modules.DebugModule.Shared.Scripts
{
    [ExecuteInEditMode]
// [SelectionBase] полезен, если вы хотите, чтобы при клике на гизмо выделялся объект GameObject
    [SelectionBase]
    public abstract class GizmoDrawerBase : MonoBehaviour
    {
        [Header("Общие настройки гизмо")] [Tooltip("Цвет, которым будет отрисован гизмо.")]
        public Color gizmoColor = Color.white;

        [Tooltip("Включить ли отображение.")]
        public bool Enable = true;

        [Tooltip("Смещение гизмо относительно центра GameObject.")]
        public Vector3 offset = Vector3.zero;

        // Этот метод автоматически вызывается Unity Editor для отрисовки гизмо.
        protected virtual void OnDrawGizmos()
        {
            // Выходим из метода если не включён скрипт
            if (!Enable)
                return;
            
            // Устанавливаем текущий цвет для всех последующих вызовов Gizmos.Draw*
            Gizmos.color = gizmoColor;

            // Вычисляем фактический центр гизмо, учитывая положение GameObject и смещение
            Vector3 gizmoCenter = transform.position + offset;

            // Вызываем абстрактный метод, который будет реализован в дочерних классах
            // для отрисовки конкретной формы.
            DrawGizmoShape(gizmoCenter);
        }

        // Абстрактный метод, который должен быть реализован в каждом дочернем классе
        // для отрисовки конкретной геометрической формы.
        protected abstract void DrawGizmoShape(Vector3 center);
    }
}