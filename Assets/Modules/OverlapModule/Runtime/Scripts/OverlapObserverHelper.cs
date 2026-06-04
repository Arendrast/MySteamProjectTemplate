#if TWO_D
using ActualCollider = UnityEngine.Collider2D;
#else
using ActualCollider = UnityEngine.Collider;
#endif
using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts
{
    public class OverlapObserverHelper : IOverlapEventsProvider
    {
        public IReadOnlyCollection<ActualCollider> CurrentOverlaps => _currentOverlaps;

        private int MaxOverlaps => _logicConfig.OverrideMaxOverlaps ? _logicConfig.MaxOverlaps : 1024;

        public event Action<IReadOnlyList<ActualCollider>> EnteredNew;
        public event Action<ActualCollider> Entered;
        public event Action<ActualCollider> Stayed;
        public event Action<ActualCollider> Exited;

        private bool _doesPerformOverlapCheck;
        
        private float _remainingTime;

        private readonly ActualCollider[] _overlapResultsBuffer;

        private readonly Func<ActualCollider[], int> _getOverlapResultsNumber;
        private readonly Action<int, List<ActualCollider>, ActualCollider[], HashSet<ActualCollider>, HashSet<ActualCollider>> _tryMakeNewOverlapsAdded;

        private readonly List<ActualCollider> _addedOverlaps = new List<ActualCollider>();
        private readonly HashSet<ActualCollider> _currentOverlaps = new HashSet<ActualCollider>();
        private readonly HashSet<ActualCollider> _previouslyOverlaps = new HashSet<ActualCollider>();

        private readonly OverlapObserverLogicConfig _logicConfig;

        public OverlapObserverHelper(OverlapObserverLogicConfig logicConfig,
            Func<ActualCollider[], int> getOverlapResultsNumber,
            Action<int, List<ActualCollider>, ActualCollider[], HashSet<ActualCollider>, HashSet<ActualCollider>> tryMakeNewOverlapsAdded)
        {
            _logicConfig = logicConfig;
            _getOverlapResultsNumber = getOverlapResultsNumber;
            _tryMakeNewOverlapsAdded = tryMakeNewOverlapsAdded;
            
            _overlapResultsBuffer = new ActualCollider[MaxOverlaps];
        }

        public void OnUpdate()
        {
            _remainingTime -= Time.deltaTime;

            if (_remainingTime > 0) return;

            PerformOverlapCheck();
            _remainingTime = _logicConfig.UpdateInterval;
        }

        public void OnEnable()
        {
            PerformOverlapCheck();
            _remainingTime = _logicConfig.UpdateInterval;
        }

        public void OnDisable()
        {
            foreach (var overlap in _currentOverlaps)
            {
                Exited?.Invoke(overlap);
            }

            _currentOverlaps.Clear();
        }

        public void PerformOverlapCheck()
        {
            if (_doesPerformOverlapCheck)
            {
                throw new Exception("Cant start next perform overlap check while performing past"); // Bugs shield
            }
            
            _doesPerformOverlapCheck = true;
            
            Array.Clear(_overlapResultsBuffer, 0, MaxOverlaps);

            _previouslyOverlaps.Clear();
            _previouslyOverlaps.AddRange(_currentOverlaps);

            _currentOverlaps.Clear();
            _addedOverlaps.Clear();

            var results = _getOverlapResultsNumber.Invoke(_overlapResultsBuffer);

            _tryMakeNewOverlapsAdded.Invoke(results, _addedOverlaps, _overlapResultsBuffer, _currentOverlaps, _previouslyOverlaps);

            foreach (var addedOverlap in _addedOverlaps)
            {
                Entered?.Invoke(addedOverlap);
            }

            if (_addedOverlaps.Count > 0)
            {
                EnteredNew?.Invoke(_addedOverlaps);
            }

            foreach (ActualCollider previousOverlap in _previouslyOverlaps)
            {
                if (!_currentOverlaps.Contains(previousOverlap))
                {
                    Exited?.Invoke(previousOverlap);
                }
            }
            
            _currentOverlaps.ForEach(overlap => Stayed?.Invoke(overlap));
            _doesPerformOverlapCheck = false;
        }
    }
}