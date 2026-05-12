using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Transporting;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.RigidbodySync
{
    public class ServerRigidbodySynchronizer : IMatchServerService
    {
        public ServerRigidbodySynchronizer(
            IServerSynchronizersMediator serverSynchronizersMediator, ServerManager serverManager)
        {
            serverSynchronizersMediator.SubscribeToBroadcast<AddForceBroadcastForServer>(
                TryAddForce);
            
            return;

            void TryAddForce(NetworkConnection connection, AddForceBroadcastForServer broadcast, Channel channel)
            {
                Debug.Log(serverManager.Objects.Spawned.GetValueOrDefault(broadcast.NetworkObjectId));
                serverManager.Objects.Spawned.GetValueOrDefault(broadcast.NetworkObjectId)?.GetComponent<Rigidbody>()?.AddForce(
                    broadcast.Force, broadcast.ForceMode);
            }
        }
    }
}