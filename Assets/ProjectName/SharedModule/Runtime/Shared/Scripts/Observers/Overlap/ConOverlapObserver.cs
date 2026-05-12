using System;
using System.Collections.Generic;
using MoreLinq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public class ConOverlapObserver : OverlapObserver
    {
        // ====== IOverlapObserver Implementation ======
        // Удаляем null-уничтоженные коллайдеры перед возвратом
        public override IReadOnlyCollection<Collider> CurrentOverlaps
        {
            get
            {
                // Удаляем любые null/уничтоженные коллайдеры перед возвратом
                _currentOverlaps.RemoveWhere(c => !c);
                return _currentOverlaps; // Преобразуем в List для IReadOnlyList
            }
        }

        private int MaxOverlaps => _overrideMaxOverlaps ? _maxOverlaps : 1000;

        public override event Action<IReadOnlyList<Collider>> EnteredNew;
        public override event Action<Collider> Entered;
        public override event Action<Collider> Stayed;
        public override event Action<Collider> Exited;

        [Header("Настройки NonAlloc")] [SerializeField]
        private bool _overrideMaxOverlaps; // Максимальное количество коллайдеров, которые могут быть обнаружены

        [ShowIf(nameof(_overrideMaxOverlaps))] [SerializeField, Min(1)]
        private int _maxOverlaps = 1000; // Максимальное количество коллайдеров, которые могут быть обнаружены

        [Header("Определение параметров сферы (локальные координаты)")] [SerializeField]
        private Vector3 _localCenter = Vector3.zero; // Локальный центр сферы относительно Transform

        [SerializeField] private Vector3 _localDirection = Vector3.forward;
        
        [SerializeField, Min(0.01f)] private float _range = 5f;
        [SerializeField, Range(0, 360f)] private float _angle = 45f;

        [Header("Определение параллелепипеда (визуализация - дебаг)")] [SerializeField]
        private Color _selectedColor = Color.yellow; // Цвет, когда объект выбран в редакторе

        [SerializeField] private Color _defaultColor = Color.green; // Цвет по умолчанию

        [Header("Настройки перекрытия")] [SerializeField]
        private LayerMask _layerMask = UnityEngine.Physics.DefaultRaycastLayers; // Маска слоев для фильтрации коллайдеров

        [SerializeField] private QueryTriggerInteraction
            _queryTriggerInteraction = QueryTriggerInteraction.UseGlobal; // Учитывать ли триггеры

        [SerializeField, Range(0.001f, 5f)]
        private float _updateInterval = 0.1f; // Как часто проверять перекрытия (в секундах)

        private float _remainingTime;
        private Collider[] _overlapResultsBuffer;

        private readonly HashSet<Collider> _currentOverlaps = new HashSet<Collider>();


        // Буфер для результатов Physics.OverlapSphereNonAlloc

        private void Awake()
        {
            // Инициализируем буферный массив при старте
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

        // Метод для установки маски слоев извне
        public void SetLayerMask(LayerMask layerMask)
        {
            _layerMask = layerMask;
        }

        // Метод для установки взаимодействия с триггерами извне
        public void SetQueryTriggerInteraction(QueryTriggerInteraction queryTriggerInteraction)
        {
            _queryTriggerInteraction = queryTriggerInteraction;
        }

        private void PerformOverlapCheck()
        {
            // Преобразуем локальные параметры сферы в мировые координаты
            Vector3 worldCenter = transform.TransformPoint(_localCenter);
            // Для сферы вращение не имеет значения, поэтому Quaternion.identity

            // Сбрасываем буфер, чтобы не обрабатывать старые результаты, если меньше maxOverlaps
            Array.Clear(_overlapResultsBuffer, 0, _overlapResultsBuffer.Length);

            var worldDirection = transform.TransformDirection(_localDirection).normalized;

            // Выполняем проверку перекрытия с помощью Physics.OverlapSphereNonAlloc
            int numOverlaps = UnityEngine.Physics.OverlapSphereNonAlloc(
                worldCenter, 
                _range, 
                _overlapResultsBuffer, 
                _layerMask, 
                _queryTriggerInteraction);

            // Создаем временный HashSet для отслеживания коллайдеров, которые перекрывались ранее,
            // и чтобы определить, какие коллайдеры вышли из области перекрытия.
            HashSet<Collider> previouslyOverlapping = new HashSet<Collider>(_currentOverlaps);

            // Очищаем текущие перекрытия для заполнения новыми данными
            _currentOverlaps.Clear();
            var addedOverlaps = new List<Collider>();

            // Обрабатываем новые обнаруженные перекрытия
            for (int i = 0; i < numOverlaps; i++)
            {
                var detectedCollider = _overlapResultsBuffer[i];

                var directionToTarget = (detectedCollider.bounds.center - worldCenter).normalized;

                // Убедимся, что коллайдер действителен (не был уничтожен)
                // RemoveWhere в CurrentOverlaps getter уже частично справляется с этим,
                // но здесь мы можем добавить проверку для предотвращения NullReferenceExceptions.
                if (directionToTarget == Vector3.zero) directionToTarget = worldDirection;

                var angleToTarget = Vector3.Angle(worldDirection, directionToTarget);

                if (angleToTarget <= _angle * 0.5f)
                {
                    if (_currentOverlaps.Add(detectedCollider))
                    {
                        // Проверяем, был ли он уже в "ранее перекрывавшихся",
                        // если он уже был там, но был временно удален из-за очистки.
                        if (!previouslyOverlapping.Contains(detectedCollider))
                        {
                            addedOverlaps.Add(detectedCollider);
                        }
                    }
                }
                // Если коллайдер был успешно добавлен (т.е. его не было в _currentOverlaps до этого вызова)
                // значит, он только что вошел в область перекрытия.
            }

            foreach (var addedOverlap in addedOverlaps)
            {
                Entered?.Invoke(addedOverlap);
            }

            if (addedOverlaps.Count > 0)
                EnteredNew?.Invoke(addedOverlaps);

            // Определяем, какие коллайдеры покинули область перекрытия
            foreach (Collider oldOverlap in previouslyOverlapping)
            {
                // Если коллайдер был в списке "ранее перекрывавшихся", но его нет в текущих,
                // значит, он покинул область.
                if (!_currentOverlaps.Contains(oldOverlap))
                {
                    Exited?.Invoke(oldOverlap);
                }
            }

            // Вызываем событие StayedCollider для всех текущих перекрытий
            _currentOverlaps.ForEach(collider => Stayed?.Invoke(collider));
        }

        // --- Визуализация в редакторе (Gizmos) ---
        private void OnDrawGizmos()
        {
            if (!enabled) return;
            Gizmos.color = _defaultColor;
            DrawCone(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _selectedColor;
            DrawCone(true);
        }

        private void DrawCone(bool selected)
        {
            Vector3 center = transform.TransformPoint(_localCenter);;
            Vector3 direction = transform.TransformDirection(_localDirection).normalized;

            // Отрисовка основной оси конуса
            Gizmos.DrawRay(center, direction * _range);

            // Отрисовка окружности основания и боковых линий конуса
            float rad = _angle * 0.5f * Mathf.Deg2Rad;
            float coneRadius = Mathf.Tan(rad) * _range;

            // Используем матрицу для удобной отрисовки круга
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(center + direction * _range, Quaternion.LookRotation(direction), Vector3.one);

            // Рисуем круг в конце конуса
            DrawWireDisk(coneRadius);

            Gizmos.matrix = oldMatrix;

            // Рисуем образующие конуса (4 линии)
            Quaternion rot = Quaternion.LookRotation(direction);
            Vector3 up = rot * Vector3.up * coneRadius;
            Vector3 right = rot * Vector3.right * coneRadius;
            Vector3 baseCenter = center + direction * _range;

            Gizmos.DrawLine(center, baseCenter + up);
            Gizmos.DrawLine(center, baseCenter - up);
            Gizmos.DrawLine(center, baseCenter + right);
            Gizmos.DrawLine(center, baseCenter - right);
        }

        private void DrawWireDisk(float radius)
        {
            float step = 20f;
            Vector3 prev = new Vector3(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius, 0);
            for (float a = step; a <= 360f; a += step)
            {
                Vector3 next = new Vector3(Mathf.Cos(a * Mathf.Deg2Rad) * radius, Mathf.Sin(a * Mathf.Deg2Rad) * radius,
                    0);
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
        }
    }
}