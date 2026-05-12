using FishNet.Broadcast;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push.Network
{
    public struct ExplodeNetworkObjectBroadcast : IBroadcast
    {
        public readonly Vector3 Direction;
        public readonly float MoveDistance;
        public readonly int NetworkObjectId;
        public readonly bool IsBlockingExplosion;

        public ExplodeNetworkObjectBroadcast(float moveDistance, Vector3 direction, int networkObjectId, bool isBlockingExplosion)
        {
            Direction = direction;
            MoveDistance = moveDistance;
            NetworkObjectId = networkObjectId;
            IsBlockingExplosion = isBlockingExplosion;
        }
    }
}