#if TWO_D
using ActualCollider = UnityEngine.Collider2D;
#else
using ActualCollider = UnityEngine.Collider;
#endif
using System;
using System.Collections.Generic;

namespace Modules.OverlapModule.Runtime.Scripts
{
    public interface IOverlapEventsProvider
    {
        event Action<ActualCollider> Entered;
        event Action<ActualCollider> Stayed;
        event Action<ActualCollider> Exited;
        event Action<IReadOnlyList<ActualCollider>> EnteredNew;
    }
}