using System;
using System.Collections.Generic;
using MoreLinq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public class SphereOverlapObserver : OverlapObserver
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
        public event Action Checked;

        [Header("Настройки NonAlloc")]
        [SerializeField] private bool _overrideMaxOverlaps; // Максимальное количество коллайдеров, которые могут быть обнаружены
        [ShowIf(nameof(_overrideMaxOverlaps))]
        [SerializeField, Min(1)] private int _maxOverlaps = 1000; // Максимальное количество коллайдеров, которые могут быть обнаружены
        
        [Header("Определение параметров сферы (локальные координаты)")]
        [SerializeField] private Vector3 _localCenter = Vector3.zero; // Локальный центр сферы относительно Transform
        [SerializeField] private float _radius = 0.5f; // Радиус сферы

        [Header("Определение параллелепипеда (визуализация - дебаг)")]
        [SerializeField] private Color _selectedColor = Color.yellow; // Цвет, когда объект выбран в редакторе
        [SerializeField] private Color _defaultColor = Color.green;   // Цвет по умолчанию

        [Header("Настройки перекрытия")]
        [SerializeField] private LayerMask _layerMask = UnityEngine.Physics.DefaultRaycastLayers; // Маска слоев для фильтрации коллайдеров
        [SerializeField] private QueryTriggerInteraction _queryTriggerInteraction = QueryTriggerInteraction.UseGlobal; // Учитывать ли триггеры

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

            // Выполняем проверку перекрытия с помощью Physics.OverlapSphereNonAlloc
            int numOverlaps = UnityEngine.Physics.OverlapSphereNonAlloc(
                worldCenter,
                _radius,
                _overlapResultsBuffer,
                _layerMask,
                _queryTriggerInteraction
            );

            // Создаем временный HashSet для отслеживания коллайдеров, которые перекрывались ранее,
            // и чтобы определить, какие коллайдеры вышли из области перекрытия.
            HashSet<Collider> previouslyOverlapping = new HashSet<Collider>(_currentOverlaps);

            // Очищаем текущие перекрытия для заполнения новыми данными
            _currentOverlaps.Clear();
            var addedOverlaps = new List<Collider>();

            // Обрабатываем новые обнаруженные перекрытия
            for (int i = 0; i < numOverlaps; i++)
            {
                Collider detectedCollider = _overlapResultsBuffer[i];

                // Убедимся, что коллайдер действителен (не был уничтожен)
                // RemoveWhere в CurrentOverlaps getter уже частично справляется с этим,
                // но здесь мы можем добавить проверку для предотвращения NullReferenceExceptions.
                if (detectedCollider == null) continue;

                // Если коллайдер был успешно добавлен (т.е. его не было в _currentOverlaps до этого вызова)
                // значит, он только что вошел в область перекрытия.
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

            Checked?.Invoke();
        }

        // --- Визуализация в редакторе (Gizmos) ---
        private void OnDrawGizmos()
        {
            if (!enabled) return;
            Gizmos.color = _defaultColor;
            DrawOverlapSphereGizmo(selected: false);
        }

        private void OnDrawGizmosSelected()
        {
            if (!enabled) return;
            Gizmos.color = _selectedColor;
            DrawOverlapSphereGizmo(selected: true);
        }

        private void DrawOverlapSphereGizmo(bool selected)
        {
            // Применяем transform компонента
            Matrix4x4 currentGizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            // Применяем локальное смещение сферы
            // Для сферы вращение не имеет значения, поэтому используем Quaternion.identity
            Matrix4x4 sphereLocalMatrix = Matrix4x4.TRS(pos: _localCenter, q: Quaternion.identity, s: Vector3.one);
            Gizmos.matrix *= sphereLocalMatrix;

            // Рисуем сферу
            if (selected)
            {
                Gizmos.DrawWireSphere(center: Vector3.zero, radius: _radius);
            }
            else
            {
                // Немного прозрачный для невыбранного состояния
                Color currentColor = Gizmos.color;
                Gizmos.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.3f);
                Gizmos.DrawSphere(center: Vector3.zero, radius: _radius);
                Gizmos.color = currentColor; // Возвращаем исходный цвет
            }

            // Восстанавливаем исходную матрицу Gizmos
            Gizmos.matrix = currentGizmoMatrix;
        }
    }
}