using FishNet.Managing.Client;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.RigidbodySync
{
    public class OwnerRigidbodySynchronizer : IMatchSharedService
    {
        public OwnerRigidbodySynchronizer(
            RigidbodySynchronizationService service,
            ClientManager clientManager)
        {
            service.SentAddForceData += SendAddForceDataData;

            return;

            void SendAddForceDataData(Vector3 force, int networkObjectId, ForceMode forceMode)
            {
                clientManager.Broadcast(new AddForceBroadcastForServer(force, forceMode, networkObjectId));
            }
        }
    }
}