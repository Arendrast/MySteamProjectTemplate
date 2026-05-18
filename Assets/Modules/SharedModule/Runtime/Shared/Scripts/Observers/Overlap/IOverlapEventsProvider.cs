using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    public interface IOverlapEventsProvider
    {
        event Action<Collider> Entered;
        event Action<Collider> Stayed;
        event Action<Collider> Exited;
        event Action<IReadOnlyList<Collider>> EnteredNew;
    }
}