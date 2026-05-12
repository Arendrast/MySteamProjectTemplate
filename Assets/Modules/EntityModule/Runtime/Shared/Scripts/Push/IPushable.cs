using System;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public interface IPushable
    {
        event Action<bool> Pushed;
        void TryPush(Vector3 explosionCenter, float forceAtCenter, float forceAtEdge, float radius, bool isBlockingExplosion, bool shouldMoveByYAxis);
        void TryPush(float moveDistance, Vector3 direction, bool isBlockingExplosion);
    }
}