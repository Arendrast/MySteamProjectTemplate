using FishNet.Managing.Client;
using FishNet.Transporting;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push.Network
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