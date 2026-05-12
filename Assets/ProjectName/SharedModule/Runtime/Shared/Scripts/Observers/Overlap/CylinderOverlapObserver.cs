using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public class CylinderOverlapObserver : OverlapObserver
    {
        // Используем IReadOnlyList для безопасного доступа извне
        public override IReadOnlyCollection<Collider> CurrentOverlaps
        {
            get
            {
                // Удаляем null/уничтоженные коллайдеры перед возвратом
                _currentOverlaps.RemoveWhere(c => c == null);
                return _currentOverlaps; // Преобразуем в List для IReadOnlyList
            }
        }

        private int MaxOverlaps => _overrideMaxOverlaps ? _maxOverlaps : 1000;
        private float Radius => Mathf.Abs(_radius);
        private float Height => Mathf.Abs(_height);
        
        
        public override event Action<IReadOnlyList<Collider>> EnteredNew;
        public override event Action<Collider> Entered;
        public override event Action<Collider> Stayed;
        public override event Action<Collider> Exited;

        [Header("Настройки NonAlloc")]
        [SerializeField] private bool _overrideMaxOverlaps; // Максимальное количество коллайдеров, которые могут быть обнаружены
        [ShowIf(nameof(_overrideMaxOverlaps))]
        [SerializeField, Min(1)] private int _maxOverlaps = 1000; // Максимальное количество коллайдеров, которые могут быть обнаружены

        public void SetLayerMask(LayerMask layerMask)
        {
            _layerMask = layerMask;
        }

        public void SetHeight(float height)
        {
            _height = height;
        }

        public void SetRadius(float radius)
        {
            _radius = radius;
        }

        public void SetQueryTriggerInteraction(QueryTriggerInteraction queryTriggerInteraction)
        {
            _queryTriggerInteraction = queryTriggerInteraction;
        }

        [Header("Определение Цилиндра (локальные координаты)")] [SerializeField]
        private Vector3 _localCenter = Vector3.zero; // Локальный центр цилиндра

        [SerializeField] private float _radius = 0.5f; // Радиус цилиндра
        [SerializeField] private float _height = 1.0f; // Высота цилиндра
        [SerializeField] private Quaternion _localRotation = Quaternion.identity; // Локальное вращение цилиндра

        [Header("Определение визуализации (визуал - дебаг)*")] [SerializeField]
        private Color _selectedColor = Color.yellow; // Цвет, когда объект выбран в редакторе

        [SerializeField] private Color _defaultColor = Color.green; // Цвет, когда объект не выбран

        [Header("Настройки перекрытия")] [SerializeField]
        private LayerMask _layerMask = UnityEngine.Physics.DefaultRaycastLayers; // Маска слоев для фильтрации коллайдеров

        [SerializeField]
        private QueryTriggerInteraction
            _queryTriggerInteraction = QueryTriggerInteraction.UseGlobal; // Учитывать ли триггеры

        [SerializeField, Range(0.001f, 5f)]
        private float _updateInterval = 0.1f; // Как часто проверять перекрытия (в секундах)

        #region Private Fields

        private float _remainingTime;
        private Collider[] _overlapResultsBuffer; // Буфер для результатов OverlapCapsuleNonAlloc
        private readonly HashSet<Collider> _currentOverlaps = new HashSet<Collider>();

        #endregion

        private void Awake()
        {
            // Инициализируем буферный массив при старте.
            // Размер буфера определяет максимальное количество коллайдеров, которые могут быть обнаружены.
            _overlapResultsBuffer = new Collider[MaxOverlaps];
        }

        private void Update()
        {
            _remainingTime -= Time.deltaTime;

            if (_remainingTime > 0) return;

            PerformOverlapCheck();
            _remainingTime = _updateInterval;
        }

        private void OnEnable()
        {
            PerformOverlapCheck();
            _remainingTime = _updateInterval;
        }

        private void OnDisable()
        {
            foreach (var overlap in _currentOverlaps)
            {
                Exited?.Invoke(overlap);
            }
            
            _currentOverlaps.Clear(); 
        }
        

        private void PerformOverlapCheck()
        {
            // 1. Преобразуем локальные параметры цилиндра в мировые координаты
            Vector3 worldCenter = transform.TransformPoint(_localCenter);
            Quaternion worldRotation = transform.rotation * _localRotation;
            Vector3 worldUpDirection = worldRotation * Vector3.up; // Направление оси цилиндра в мировых координатах

            // Расчет точек для Physics.OverlapCapsuleNonAlloc
            // point1 и point2 определяют центральную линию капсулы.
            // Для имитации цилиндра, эти точки должны быть центрами верхнего и нижнего "полушарий" капсулы.
            // Поскольку капсула имеет полусферы на концах, мы хотим, чтобы ее центральный стержень
            // был равен высоте цилиндра, а "полусферы" совпадали с радиусом цилиндра.
            // Таким образом, общая высота капсулы = высота_стержня + 2 * радиус.
            // Для "цилиндрической" капсулы, стержень должен иметь длину,
            // при которой он простирается между центрами верхнего и нижнего кругов.

            // Высота от центра до верхнего/нижнего диска
            float halfHeight = Height / 2f;

            // Точки для капсулы, если бы она полностью имитировала цилиндр:
            // Одна точка находится на полпути от центра вверх, другая на полпути вниз.
            // С учетом радиуса, эти точки должны быть смещены на halfHeight - radius.
            // Важно: если _height <= 2 * _radius, то капсула будет сферой или очень короткой капсулой.
            // Чтобы получить "прямой" цилиндр, мы должны сделать центральный стержень капсулы равным _height,
            // а радиус _radius. Это означает, что конечные точки капсулы будут центрами
            // полусфер, которые выступают за высоту цилиндра.
            // Чтобы приблизиться к цилиндру, точки должны быть на halfHeight - _radius от центра.
            // Однако, для простоты, и чтобы капсула максимально покрывала объем,
            // мы можем использовать точки, которые лежат на halfHeight от центра.
            // Это сделает капсулу немного "выше" цилиндра на 2 * radius.
            // Чтобы получить точно "высоту" цилиндра, нужно установить точки на `halfHeight - _radius`
            // Однако, Unity интерпретирует `point1` и `point2` как центры полусфер.
            // Так что для капсулы, которая по высоте равна цилиндру, точки должны быть:
            Vector3 capsulePoint1 = worldCenter + worldUpDirection * (halfHeight - Radius);
            Vector3 capsulePoint2 = worldCenter - worldUpDirection * (halfHeight - Radius);

            // Если _height <= 2*_radius, то капсула становится сферой.
            // В этом случае, point1 и point2 могут совпадать или быть инвертированы.
            // OverlapCapsuleNonAlloc хорошо справляется с этим.
            if (Height <= 2f * Radius)
            {
                // Если высота меньше или равна удвоенному радиусу, то это фактически сфера или очень короткая капсула.
                // В этом случае лучше просто центрировать точки.
                capsulePoint1 = worldCenter;
                capsulePoint2 = worldCenter;
            }


            // 2. Сбрасываем буфер, чтобы не обрабатывать старые результаты
            Array.Clear(_overlapResultsBuffer, 0, MaxOverlaps);

            // 3. Выполняем проверку перекрытия с помощью Physics.OverlapCapsuleNonAlloc
            int numOverlaps = UnityEngine.Physics.OverlapCapsuleNonAlloc(
                capsulePoint1,
                capsulePoint2,
                Radius,
                _overlapResultsBuffer,
                _layerMask,
                _queryTriggerInteraction
            );

            // 4. Создаем временный HashSet для отслеживания коллайдеров, которые перекрывались ранее
            HashSet<Collider> previouslyOverlapping = new HashSet<Collider>(_currentOverlaps);
            _currentOverlaps.Clear(); // Очищаем текущие перекрытия для заполнения новыми данными

            List<Collider> addedOverlaps = new List<Collider>();

            // 5. Обрабатываем новые обнаруженные перекрытия
            for (var i = 0; i < numOverlaps; i++)
            {
                Collider detectedCollider = _overlapResultsBuffer[i];

                if (detectedCollider == null) continue;

                _currentOverlaps.Add(detectedCollider);

                if (!previouslyOverlapping.Contains(detectedCollider))
                {
                    addedOverlaps.Add(detectedCollider);
                }
            }

            foreach (var addedOverlap in addedOverlaps)
            {
                Entered?.Invoke(addedOverlap);
            }
            
            if (addedOverlaps.Count > 0)
                EnteredNew?.Invoke(addedOverlaps);

            // 6. Определяем, какие коллайдеры покинули область перекрытия
            foreach (Collider oldOverlap in previouslyOverlapping)
            {
                if (!_currentOverlaps.Contains(oldOverlap))
                {
                    Exited?.Invoke(oldOverlap);
                }
            }

            // 7. Все, что осталось в _currentOverlaps, означает, что оно осталось в области перекрытия
            foreach (Collider stillOverlapping in _currentOverlaps)
            {
                if (previouslyOverlapping.Contains(stillOverlapping))
                {
                    Stayed?.Invoke(stillOverlapping);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!enabled) return;

            Gizmos.color = _defaultColor;
            DrawOverlapCylinderGizmo(false);
        }

        private void OnDrawGizmosSelected()
        {
            if (!enabled) return;

            Gizmos.color = _selectedColor;
            DrawOverlapCylinderGizmo(true);
        }

        private void DrawOverlapCylinderGizmo(bool selected)
        {
            // Применяем transform компонента
            Matrix4x4 currentGizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            // Применяем локальное смещение и вращение цилиндра
            Matrix4x4 cylinderLocalMatrix = Matrix4x4.TRS(_localCenter, _localRotation, Vector3.one);
            Gizmos.matrix *= cylinderLocalMatrix;

            // Цвет
            Color currentGizmoColor = Gizmos.color;

            // Рисуем цилиндр
            DrawWireCylinder(Radius, Height);

            // Восстанавливаем исходный цвет и матрицу Gizmos
            Gizmos.color = currentGizmoColor;
            Gizmos.matrix = currentGizmoMatrix;
        }

        // Вспомогательная функция для отрисовки каркасного цилиндра в локальных координатах
        private void
            DrawWireCylinder(float radius, float height,
                int segments = 20) // Уменьшил сегменты для более быстрого рендера
        {
            Vector3 topCenter = Vector3.up * (height / 2f);
            Vector3 bottomCenter = Vector3.down * (height / 2f);

            // Рисуем верхнюю и нижнюю окружности
            DrawWireCircle(topCenter, Quaternion.identity, radius, segments);
            DrawWireCircle(bottomCenter, Quaternion.identity, radius, segments);

            // Рисуем вертикальные линии, соединяющие окружности
            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(topCenter + offset, bottomCenter + offset);
                Popcron.Gizmos.Line(topCenter + offset, bottomCenter + offset, Gizmos.color);
            }
        }

        // Вспомогательная функция для отрисовки каркасной окружности в локальных координатах
        private void DrawWireCircle(Vector3 center, Quaternion rotation, float radius, int segments)
        {
            Vector3 prevPoint = center + rotation * new Vector3(radius, 0, 0);
            for (int i = 1; i <= segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                Vector3 currentPoint =
                    center + rotation * new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(prevPoint, currentPoint);
                Popcron.Gizmos.Line(prevPoint, currentPoint, Gizmos.color);
                prevPoint = currentPoint;
            }
        }
    }
}