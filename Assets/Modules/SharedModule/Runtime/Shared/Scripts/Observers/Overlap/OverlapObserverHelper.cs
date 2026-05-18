using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public class OverlapObserverHelper : IOverlapEventsProvider
    {
        public IReadOnlyCollection<Collider> CurrentOverlaps => _currentOverlaps;

        private int MaxOverlaps => _logicConfig.OverrideMaxOverlaps ? _logicConfig.MaxOverlaps : 1024;

        public event Action<IReadOnlyList<Collider>> EnteredNew;
        public event Action<Collider> Entered;
        public event Action<Collider> Stayed;
        public event Action<Collider> Exited;

        private bool _doesPerformOverlapCheck;
        
        private float _remainingTime;

        private readonly Collider[] _overlapResultsBuffer;

        private readonly Func<Collider[], int> _getOverlapResultsNumber;
        private readonly Action<int, List<Collider>, Collider[], HashSet<Collider>, HashSet<Collider>> _tryMakeNewOverlapsAdded;

        private readonly List<Collider> _addedOverlaps = new List<Collider>();
        private readonly HashSet<Collider> _currentOverlaps = new HashSet<Collider>();
        private readonly HashSet<Collider> _previouslyOverlaps = new HashSet<Collider>();

        private readonly OverlapObserverLogicConfig _logicConfig;

        public OverlapObserverHelper(OverlapObserverLogicConfig logicConfig,
            Func<Collider[], int> getOverlapResultsNumber,
            Action<int, List<Collider>, Collider[], HashSet<Collider>, HashSet<Collider>> tryMakeNewOverlapsAdded)
        {
            _logicConfig = logicConfig;
            _getOverlapResultsNumber = getOverlapResultsNumber;
            _tryMakeNewOverlapsAdded = tryMakeNewOverlapsAdded;

            _overlapResultsBuffer = new Collider[MaxOverlaps];
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

            foreach (Collider previousOverlap in _previouslyOverlaps)
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