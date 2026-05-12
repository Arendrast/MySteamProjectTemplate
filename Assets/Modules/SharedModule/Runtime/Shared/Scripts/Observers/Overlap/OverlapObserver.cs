using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public abstract class OverlapObserver : MonoBehaviour, IOverlapObserver
    {
        public abstract IReadOnlyCollection<Collider> CurrentOverlaps { get; }
        public abstract event Action<Collider> Entered;
        public abstract event Action<Collider> Stayed;
        public abstract event Action<Collider> Exited;
        public abstract event Action<IReadOnlyList<Collider>> EnteredNew;
    }
}