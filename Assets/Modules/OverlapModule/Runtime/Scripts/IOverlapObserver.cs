using System.Collections.Generic;

#if TWO_D
using ActualCollider = UnityEngine.Collider2D;
#else
using ActualCollider = UnityEngine.Collider;
#endif

namespace Modules.OverlapModule.Runtime.Scripts
{
    public interface IOverlapObserver
    {
        IReadOnlyCollection<ActualCollider> CurrentOverlaps { get; }
        IOverlapEventsProvider EventsProvider { get; }
    }
}