#if TWO_D
using ActualCollider = UnityEngine.Collider2D;
#else
using ActualCollider = UnityEngine.Collider;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts
{
    [ExecuteInEditMode]
    public abstract class OverlapObserver : MonoBehaviour, IOverlapObserver
    {
        public IReadOnlyCollection<ActualCollider> CurrentOverlaps => ObserverHelper.CurrentOverlaps;
        public IOverlapEventsProvider EventsProvider => ObserverHelper;
        public OverlapObserverModel SharedModel { get; private set; }
        protected OverlapObserverHelper ObserverHelper { get; private set; }
        protected OverlapGizmosDrawer OverlapGizmosDrawer { get; private set; }

        [SerializeField] private OverlapObserverLogicConfig _overlapConfig;
        [SerializeField] private OverlapObserverDebugConfig _debugConfig;

        protected virtual void Awake()
        {
            OverlapGizmosDrawer = new OverlapGizmosDrawer(_debugConfig, DrawGizmos);
            ObserverHelper = new OverlapObserverHelper(_overlapConfig, GetAddedOverlapResultsNumber,
                TryMakeNewOverlapAdded);
            SharedModel = new OverlapObserverModel(_overlapConfig);
        }
        
        private void OnDrawGizmos()
        {
            if (enabled)
            {
                OverlapGizmosDrawer.DrawGizmos(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (enabled)
            {
                OverlapGizmosDrawer.DrawGizmos(true);
            }
        }
        
        private void Update() => ObserverHelper.OnUpdate();
        private void OnEnable() => ObserverHelper.OnEnable();
        private void OnDisable() => ObserverHelper.OnDisable();

        public void PerformOverlapCheck() => ObserverHelper.PerformOverlapCheck();

        protected abstract void DrawGizmos(bool obj);
        protected abstract int GetAddedOverlapResultsNumber(ActualCollider[] overlapResultsBuffer);

        protected virtual void TryMakeNewOverlapAdded(int numberOverlaps, List<ActualCollider> addedOverlaps,
            ActualCollider[] overlapResultsBuffer, HashSet<ActualCollider> currentOverlaps,
            HashSet<ActualCollider> previouslyOverlaps)
        {
            for (int i = 0; i < numberOverlaps; i++)
            {
                ActualCollider overlap = overlapResultsBuffer[i];

                if (currentOverlaps.Add(overlap))
                {
                    if (!previouslyOverlaps.Contains(overlap))
                    {
                        addedOverlaps.Add(overlap);
                    }
                }
            }
        }
    }
}