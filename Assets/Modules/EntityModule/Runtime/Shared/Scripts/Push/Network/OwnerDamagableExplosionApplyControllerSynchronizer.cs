using FishNet.Managing.Client;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push.Network
{
    public class OwnerDamagableExplosionApplyControllerSynchronizer : IMatchSharedService
    {
        public OwnerDamagableExplosionApplyControllerSynchronizer(
            DamagableExplosionApplyControllersSynchronizationService service,
            ClientManager clientManager)
        {
            service.SentData += SendData;

            return;

            void SendData(ExplosionData data, int damageDealerId)
            {
                clientManager.Broadcast(new CreateAndApplyDamagableExplosionApplyControllerBroadcast(
                    data.WithExcludedGameObjects(null),
                    damageDealerId));
            }
        }
    }
}