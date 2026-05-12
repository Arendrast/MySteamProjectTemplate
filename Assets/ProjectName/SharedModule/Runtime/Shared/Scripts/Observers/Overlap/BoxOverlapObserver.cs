using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public class BoxOverlapObserver : OverlapObserver
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

        [Header("Настройки NonAlloc")] [SerializeField]
        private bool _overrideMaxOverlaps; // Максимальное количество коллайдеров, которые могут быть обнаружены

        [ShowIf(nameof(_overrideMaxOverlaps))] [SerializeField, Min(1)]
        private int _maxOverlaps = 1000; // Максимальное количество коллайдеров, которые могут быть обнаружены

        [Header("Определение Параллелепипеда (локальные координаты)")] [SerializeField]
        private Vector3 _localCenter = Vector3.zero; // Локальный центр параллелепипеда

        [SerializeField]
        private Vector3 _localHalfExtents = new Vector3(0.5f, 0.5f, 0.5f); // Половина размера по каждой оси (локальные)

        [SerializeField] private Quaternion _localRotation = Quaternion.identity; // Локальное вращение параллелепипеда

        [Header("Определение Параллелепипеда (визуально - дебаг)")] [SerializeField]
        private Color _selectedColor = Color.yellow;

        [SerializeField] private Color _defaultColor = Color.green;

        [Header("Настройки Перекрытия")] [SerializeField]
        private LayerMask _layerMask = UnityEngine.Physics.DefaultRaycastLayers; // Маска слоев для фильтрации коллайдеров

        [SerializeField] private QueryTriggerInteraction
            _queryTriggerInteraction = QueryTriggerInteraction.UseGlobal; // Учитывать ли триггеры

        [SerializeField] private bool _boxCastFromLastPosition;

        [SerializeField, Range(0.001f, 5f)]
        private float _updateInterval = 0.1f; // Как часто проверять перекрытия (в секундах)

        private Collider[] _overlapResultsBuffer;
        private RaycastHit[] _castResultsBuffer;
        private Vector3? _lastCastPosition;
        private float _remainingTime;

        private readonly HashSet<Collider> _currentOverlaps = new HashSet<Collider>();


        private void Awake()
        {
            // Инициализируем буферный массив при старте.
            // Размер буфера определяет максимальное количество коллайдеров, которые могут быть обнаружены.
            _overlapResultsBuffer = new Collider[MaxOverlaps];
            _castResultsBuffer = new RaycastHit[MaxOverlaps];
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
            foreach (var overlap in _currentOverlaps.ToList())
            {
                Exited?.Invoke(overlap);
            }

            _currentOverlaps.Clear();
        }

        public void SetLayerMask(LayerMask layerMask)
        {
            _layerMask = layerMask;
        }

        public void SetQueryTriggerInteraction(QueryTriggerInteraction queryTriggerInteraction)
        {
            _queryTriggerInteraction = queryTriggerInteraction;
        }


        public Vector3 GetCheckCenter()
        {
            return transform.TransformPoint(_localCenter);
        }

        public void PerformOverlapCheck()
        {
            // Преобразуем локальные параметры параллелепипеда в мировые координаты
            Vector3 worldCenter = GetCheckCenter();
            // Мировое вращение параллелепипеда - это вращение родительского объекта
            // умноженное на локальное вращение параллелепипеда.
            Quaternion worldRotation = transform.rotation * _localRotation;

            // Сбрасываем буфер, чтобы не обрабатывать старые результаты, если новых меньше maxOverlaps
            Array.Clear(_overlapResultsBuffer, 0, MaxOverlaps);

            var numOverlaps = 0;

            // Выполняем проверку перекрытия с помощью Physics.OverlapBoxNonAlloc

            if (!_boxCastFromLastPosition || (_boxCastFromLastPosition && _lastCastPosition == null))
            {
                numOverlaps = UnityEngine.Physics.OverlapBoxNonAlloc(
                    worldCenter,
                    _localHalfExtents.Multiply(transform.lossyScale)
                        .Abs(),
                    _overlapResultsBuffer,
                    worldRotation,
                    _layerMask,
                    _queryTriggerInteraction);

                if (_boxCastFromLastPosition)
                {
                    _lastCastPosition = worldCenter;
                }
            }
            else
            {
                var direction = worldCenter - _lastCastPosition.Value;
                var distance = direction.magnitude;

                numOverlaps = UnityEngine.Physics.BoxCastNonAlloc(
                    _lastCastPosition.Value,
                    _localHalfExtents.Multiply(transform.lossyScale)
                        .Abs(),
                    direction.normalized,
                    _castResultsBuffer,
                    transform.rotation,
                    distance,
                    _layerMask, _queryTriggerInteraction
                );

                if (name.StartsWith("Box"))
                    GizmoTools.DrawBoxCast(worldCenter, _localHalfExtents.Multiply(transform.lossyScale).Abs(),
                        direction.normalized, transform.rotation, distance, 10);

                _lastCastPosition = worldCenter;
            }

            // Создаем временный HashSet для отслеживания коллайдеров, которые перекрывались ранее,
            // чтобы определить, какие коллайдеры вышли из области перекрытия.
            HashSet<Collider> previouslyOverlapping = new HashSet<Collider>(_currentOverlaps);

            // Очищаем текущие перекрытия для заполнения новыми данными
            _currentOverlaps.Clear();

            var addedOverlaps = new List<Collider>();

            // Обрабатываем новые обнаруженные перекрытия
            for (int i = 0; i < numOverlaps; i++)
            {
                Collider detectedCollider =
                    _boxCastFromLastPosition ? _castResultsBuffer[i].collider : _overlapResultsBuffer[i];

                // Убедимся, что коллайдер действителен (не был уничтожен)
                // RemoveWhere в CurrentOverlaps getter уже частично справляется с этим,
                // но здесь мы можем добавить проверку для предотвращения NullReferenceExceptions.
                if (detectedCollider == null) continue;

                // Если коллайдер был успешно добавлен (т.е. его не было в _currentOverlaps до этого вызова Add),
                // значит, он только что вошел в область перекрытия.
                if (_currentOverlaps.Add(detectedCollider))
                {
                    // Проверяем, был ли он уже в "ранее перекрывающихся", чтобы не вызывать EnteredCollider,
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
                // Если коллайдер был в списке "ранее перекрывающихся", но его нет в текущих,
                // значит, он покинул область.
                if (!_currentOverlaps.Contains(oldOverlap))
                {
                    Exited?.Invoke(oldOverlap);
                }
            }

            _currentOverlaps.ForEach(overlap => Stayed?.Invoke(overlap));
        }

        // --- Визуализация в редакторе (Gizmos) ---
        private void OnDrawGizmos()
        {
            if (enabled)
            {
                Gizmos.color = _defaultColor;
                DrawOverlapBoxGizmo(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _selectedColor;
            DrawOverlapBoxGizmo(true);
        }

        private void DrawOverlapBoxGizmo(bool selected)
        {
            // Применяем transform компонента
            Matrix4x4 currentGizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            // Применяем локальное смещение и вращение коробки
            Matrix4x4 boxLocalMatrix = Matrix4x4.TRS(_localCenter, _localRotation, Vector3.one);
            Gizmos.matrix *= boxLocalMatrix;

            // Рисуем куб
            if (selected)
            {
                Gizmos.DrawWireCube(Vector3.zero, _localHalfExtents * 2); // halfExtents * 2 = полный размер
            }
            else
            {
                // Немного прозрачный для невыбранного состояния
                Color current = Gizmos.color;
                Gizmos.color = new Color(current.r, current.g, current.b, 0.3f);
                Gizmos.DrawCube(Vector3.zero, _localHalfExtents * 2);
                Gizmos.color = current; // Возвращаем исходный цвет
            }

            // Восстанавливаем исходную матрицу Gizmos
            Gizmos.matrix = currentGizmoMatrix;
        }
    }
}