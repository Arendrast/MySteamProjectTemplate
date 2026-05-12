using FishNet.Managing.Client;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push.Network
{
    public class OwnerExplodeNetworkObjectSynchronizer : IMatchSharedService
    {
        public OwnerExplodeNetworkObjectSynchronizer(
            ExplodeNetworkObjectSynchronizationService service, ClientManager clientManager)
        {
            service.SentData += SendData;

            return;

            void SendData(float moveDistance, Vector3 direction, int networkObjectId, bool isBlockingExplosion)
            {
                clientManager.Broadcast(
                    new ExplodeNetworkObjectBroadcast(moveDistance, direction, networkObjectId, isBlockingExplosion));
            }
        }
    }
}