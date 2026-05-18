using System.Collections.Generic;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public abstract class OverlapObserver : MonoBehaviour, IOverlapObserver
    {
        public IReadOnlyCollection<Collider> CurrentOverlaps => ObserverHelper.CurrentOverlaps;
        public IOverlapEventsProvider EventsProvider => ObserverHelper;
        protected OverlapObserverHelper ObserverHelper { get; private set; }
        protected OverlapGizmosDrawer OverlapGizmosDrawer { get; private set; }
        
        [field: SerializeField] protected OverlapObserverLogicConfig OverlapConfig { get; private set; }
        [SerializeField] private OverlapObserverDebugConfig _debugConfig;

        private void Awake()
        {
            ObserverHelper = new OverlapObserverHelper(OverlapConfig, GetAddedOverlapResultsNumber,
                TryMakeNewOverlapAdded);

            OverlapGizmosDrawer = new OverlapGizmosDrawer(_debugConfig, DrawGizmos);
        }

        private void Update() => ObserverHelper.OnUpdate();
        private void OnEnable() => ObserverHelper.OnEnable();
        private void OnDisable() => ObserverHelper.OnDisable();
        private void OnDrawGizmos() => OverlapGizmosDrawer.DrawGizmos(false);
        private void OnDrawGizmosSelected() => OverlapGizmosDrawer.DrawGizmos(true);
        
        public void PerformOverlapCheck() => ObserverHelper.PerformOverlapCheck();
        
        protected abstract void DrawGizmos(bool obj);
        protected abstract int GetAddedOverlapResultsNumber(Collider[] overlapResultsBuffer);

        protected virtual void TryMakeNewOverlapAdded(int numberOverlaps, List<Collider> addedOverlaps,
            Collider[] overlapResultsBuffer, HashSet<Collider> currentOverlaps,
            HashSet<Collider> previouslyOverlaps)
        {
            for (int i = 0; i < numberOverlaps; i++)
            {
                Collider overlap = overlapResultsBuffer[i];

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