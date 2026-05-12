using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public class CapsuleOverlapObserver : OverlapObserver
    {
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

        private float Radius => Mathf.Abs(_radius);

        [Header("Настройки NonAlloc")]
        [SerializeField] private bool _overrideMaxOverlaps; // Максимальное количество коллайдеров, которые могут быть обнаружены
        [ShowIf(nameof(_overrideMaxOverlaps))]
        [SerializeField, Min(1)] private int _maxOverlaps = 1000; // Максимальное количество коллайдеров, которые могут быть обнаружены

        [Header("Определение Капсулы (локальные координаты)")] [SerializeField]
        private Vector3 _localPoint0 = new Vector3(0, -0.5f, 0); // Локальная стартовая точка капсулы

        [SerializeField] private Vector3 _localPoint1 = new Vector3(0, 0.5f, 0); // Локальная конечная точка капсулы
        [SerializeField, Range(0.01f, 10f)] private float _radius = 0.5f;

        [Header("Настройки Перекрытия")] [SerializeField]
        private LayerMask _layerMask = UnityEngine.Physics.DefaultRaycastLayers; // Маска слоев для фильтрации коллайдеров

        [SerializeField] private QueryTriggerInteraction
            _queryTriggerInteraction = QueryTriggerInteraction.UseGlobal; // Учитывать ли триггеры

        [Header("Определение Параллелепипеда (визуально - дебаг)")] [SerializeField]
        private Color _selectedColor = Color.yellow;

        [SerializeField] private Color _defaultColor = Color.green;

        [SerializeField, Range(0.001f, 5f)]
        private float _updateInterval = 0.1f; // Как часто проверять перекрытия (в секундах)

        private float _remainingTime;

        // Буфер для результатов OverlapCapsuleNonAlloc
        private Collider[] _overlapResultsBuffer;
        
        private Vector3 _defaultLocalPoint0, _defaultLocalPoint1;
        private readonly HashSet<Collider> _currentOverlaps = new HashSet<Collider>();

        private void Awake()
        {
            // Инициализируем буферный массив при старте.
            // Размер буфера определяет максимальное количество коллайдеров, которые могут быть обнаружены.
            _overlapResultsBuffer = new Collider[MaxOverlaps];

            _defaultLocalPoint0 = _localPoint0;
            _defaultLocalPoint1 = _localPoint1;
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
        
        public void SetLocalPoints(Vector3 localPoint0, Vector3 localPoint1)
        {
            _localPoint0 = localPoint0;
            _localPoint1 = localPoint1;
        }

        public void SetLocalPointsToDefault()
        {
            SetLocalPoints(_defaultLocalPoint0, _defaultLocalPoint1);
        }

        private void PerformOverlapCheck()
        {
            // Преобразуем локальные точки капсулы в мировые координаты
            Vector3 worldPoint0 = transform.TransformPoint(_localPoint0);
            Vector3 worldPoint1 = transform.TransformPoint(_localPoint1);

            // Используем Physics.OverlapCapsuleNonAlloc для получения коллайдеров.
            // Метод возвращает количество найденных коллайдеров и заполняет _overlapResultsBuffer.
            int numHits = UnityEngine.Physics.OverlapCapsuleNonAlloc(worldPoint0, worldPoint1, Radius, _overlapResultsBuffer,
                _layerMask, _queryTriggerInteraction);

            // Создаем временный HashSet из текущих результатов для сравнения
            List<Collider> newOverlaps = new List<Collider>();
            for (int i = 0; i < numHits; i++)
            {
                Collider hitCollider = _overlapResultsBuffer[i];
                if (hitCollider !=
                    null) // Убедимся, что коллайдер не null (хотя NonAlloc обычно не оставляет null до numHits)
                {
                    newOverlaps.Add(hitCollider);
                }

                // Очищаем ссылку в буфере сразу, чтобы не удерживать объект дольше, чем нужно.
                // Это важно, если коллайдеры могут быть уничтожены между проверками.
                _overlapResultsBuffer[i] = null;
            }

            // 1. Идентифицируем вышедшие коллайдеры
            // Создаем временный список, чтобы избежать изменения коллекции _currentOverlaps во время итерации
            foreach (Collider oldCollider in _currentOverlaps.ToList())
            {
                if (!oldCollider) // Если коллайдер был уничтожен вне системы
                {
                    _currentOverlaps.Remove(oldCollider);
                    continue;
                }

                if (!newOverlaps.Contains(oldCollider))
                {
                    _currentOverlaps.Remove(oldCollider);
                    Exited?.Invoke(oldCollider);
                }
            }

            // 2. Идентифицируем вошедшие коллайдеры
            foreach (Collider newCollider in newOverlaps)
            {
                if (!_currentOverlaps.Contains(newCollider))
                {
                    _currentOverlaps.Add(newCollider);
                    Entered?.Invoke(newCollider);
                }
            }
            
            if (newOverlaps.Count > 0)
                EnteredNew?.Invoke(newOverlaps);

            foreach (var overlap in _currentOverlaps)
            {
                Stayed?.Invoke(overlap);
            }
        }

        // --- Gizmos для визуализации капсулы в редакторе ---
        private void OnDrawGizmos()
        {
            DrawCapsuleGizmo(_defaultColor); // Цвет по умолчанию
        }

        private void OnDrawGizmosSelected()
        {
            DrawCapsuleGizmo(_selectedColor); // Цвет при выборе объекта
        }

        private void DrawCapsuleGizmo(Color color)
        {
            if (!enabled || !gameObject.activeInHierarchy) return;

            // Преобразуем локальные точки в мировые
            Vector3 worldPoint0 = transform.TransformPoint(_localPoint0);
            Vector3 worldPoint1 = transform.TransformPoint(_localPoint1);

            Gizmos.color = color;

            // Отсутствует встроенный Gizmos.DrawCapsule, поэтому рисуем вручную:
            Gizmos.DrawWireSphere(worldPoint0, Radius);
            Gizmos.DrawWireSphere(worldPoint1, Radius);

            Vector3 direction = (worldPoint1 - worldPoint0).normalized;
            // Находим два перпендикулярных вектора
            Quaternion rotation = Quaternion.LookRotation(direction);
            Vector3 side1 = rotation * Vector3.right * Radius;
            Vector3 side2 = rotation * Vector3.up * Radius;

            // Рисуем линии, соединяющие "бока" капсулы
            Gizmos.DrawLine(worldPoint0 + side1, worldPoint1 + side1);
            Gizmos.DrawLine(worldPoint0 - side1, worldPoint1 - side1);
            Gizmos.DrawLine(worldPoint0 + side2, worldPoint1 + side2);
            Gizmos.DrawLine(worldPoint0 - side2, worldPoint1 - side2);

            // Для более полного отображения можно добавить больше линий,
            // но для простоты этого достаточно.
        }
    }
}