using System;
using System.Collections.Generic;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers
{
    public interface IOverlapObserver
    {
        IReadOnlyCollection<Collider> CurrentOverlaps { get; }
        IOverlapEventsProvider EventsProvider { get; }
    }
}