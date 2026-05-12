using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push.Network
{
    public class ExplodeNetworkObjectSynchronizationService : IMatchSharedService
    {
        public event Action<float, Vector3, int, bool> SentData;

        public void Send(float moveDistance, Vector3 direction, int networkObjectId, bool isBlockingExplosion)
        {
            SentData?.Invoke(moveDistance, direction, networkObjectId, isBlockingExplosion);
        }
    }
}