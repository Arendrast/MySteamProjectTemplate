using FishNet.Managing.Client;
using FishNet.Transporting;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push.Network
{
    public class ClientsExplodeNetworkObjectSynchronizer : IMatchSharedService
    {
        public ClientsExplodeNetworkObjectSynchronizer(
            IClientsSynchronizersMediator clientsSynchronizersMediator,
            PushablesRepository pushablesRepository, ClientManager clientManager)
        {
            clientsSynchronizersMediator
                .SubscribeToBroadcast<ExplodeNetworkObjectBroadcast>(
                    HandleBroadcast);

            return;

            void HandleBroadcast(ExplodeNetworkObjectBroadcast broadcast,
                Channel channel)
            {
                var explodableSerializableComponents = clientManager.TryGetNetworkObjectById(broadcast.NetworkObjectId)?.GetComponent<ExplodableSerializableComponents>();
                
                if (explodableSerializableComponents == null || !pushablesRepository.TryGetValue(explodableSerializableComponents, out var explodable))
                    return;
                
                explodable.TryPush(broadcast.MoveDistance, broadcast.Direction, broadcast.IsBlockingExplosion);
            }
        }
    }
}