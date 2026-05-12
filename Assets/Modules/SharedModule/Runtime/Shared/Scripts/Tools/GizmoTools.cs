using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class GizmoTools
    {
        public static void DrawBoxCast(Vector3 origin, Vector3 halfExtents, Vector3 direction, Quaternion rotation,
            float distance, float time = 0f)
        {
            Popcron.Gizmos.Cube(origin, rotation, halfExtents * 2, Color.red, time: time);
            Popcron.Gizmos.Cube(origin + direction * distance, rotation, halfExtents * 2, Color.red, time: time);

            // Рисуем соединительные линии между углами
            var corners = GetBoxCorners(halfExtents, origin);
        
            // Локальное направление движения для линий
            var localDir = Quaternion.Inverse(rotation) * (direction * distance);

            foreach (var corner in corners)
            {
                Popcron.Gizmos.Line(corner, corner + localDir, Color.red, time: time);
            }
        }

        public static void DrawSphereCast(RaycastHit hit, Vector3 origin, float maxDistance, Vector3 direction,
            float radius, Color color, float time = 0f)
        {
            float currentDistance = hit.collider != null ? hit.distance : maxDistance;
            Vector3 endPosition = origin + direction * currentDistance;
            
            Popcron.Gizmos.Sphere(origin, radius, color, time: time);

            // 2. Рисуем конечную сферу (или сферу в месте удара)
            Popcron.Gizmos.Sphere(endPosition, radius, color, time: time);

            // 3. Рисуем соединительные линии (чтобы видеть объем каста)
            DrawSphereCastLines(origin, endPosition, radius, direction, color, time);

            // 4. Дополнительно: рисуем нормаль в точке попадания
            if (hit.collider != null)
            {
                Popcron.Gizmos.Line(hit.point, hit.point + hit.normal * 0.5f, Color.yellow, time: time);
            }
        }

        private static Vector3[] GetBoxCorners(Vector3 extents, Vector3 origin)
        {
            return new Vector3[]
            {
                new Vector3( extents.x,  extents.y,  extents.z) + origin,
                new Vector3( extents.x,  extents.y, -extents.z) + origin,
                new Vector3( extents.x, -extents.y,  extents.z) + origin,
                new Vector3( extents.x, -extents.y, -extents.z) + origin,
                new Vector3(-extents.x,  extents.y,  extents.z) + origin,
                new Vector3(-extents.x,  extents.y, -extents.z) + origin,
                new Vector3(-extents.x, -extents.y,  extents.z) + origin,
                new Vector3(-extents.x, -extents.y, -extents.z) + origin,
            };
        }

        private static void DrawSphereCastLines(Vector3 start, Vector3 end, float radius, Vector3 direction,
            Color color, float time)
        {
            // Находим перпендикулярные векторы для отрисовки боковых линий
            Vector3 up = Vector3.up;
            if (Mathf.Abs(Vector3.Dot(direction, Vector3.up)) > 0.9f)
                up = Vector3.right; // Чтобы избежать коллинеарности

            Vector3 right = Vector3.Cross(direction, up).normalized * radius;
            up = Vector3.Cross(right, direction).normalized * radius;

            // Рисуем 4 линии вдоль «трубы» каста
            Popcron.Gizmos.Line(start + right, end + right, color, time: time);
            Popcron.Gizmos.Line(start - right, end - right, color, time: time);
            Popcron.Gizmos.Line(start + up, end + up, color, time: time);
            Popcron.Gizmos.Line(start - up, end - up, color, time: time);
        }
    }
}