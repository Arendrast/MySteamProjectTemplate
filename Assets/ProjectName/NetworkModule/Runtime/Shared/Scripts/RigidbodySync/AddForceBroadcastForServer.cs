using FishNet.Broadcast;
using UnityEngine;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.RigidbodySync
{
    public readonly struct AddForceBroadcastForServer : IBroadcast
    {
        public readonly Vector3 Force;
        public readonly ForceMode ForceMode;
        public readonly int NetworkObjectId;

        public AddForceBroadcastForServer(Vector3 force, ForceMode forceMode, int networkObjectId)
        {
            Force = force;
            ForceMode = forceMode;
            NetworkObjectId = networkObjectId;
        }
    }
}