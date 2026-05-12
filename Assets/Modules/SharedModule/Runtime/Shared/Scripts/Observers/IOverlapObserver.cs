using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers
{
    public interface IOverlapObserver
    {
        IReadOnlyCollection<Collider> CurrentOverlaps { get; }
        
        event Action<Collider> Entered;
        event Action<Collider> Stayed;
        event Action<Collider> Exited;
    }
}