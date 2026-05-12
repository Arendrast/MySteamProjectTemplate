using FishNet.Transporting;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Configs;
using ProjectName.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push.Network
{
    public class ClientsDamagableExplosionApplyControllersSynchronizer : IMatchSharedService
    {
        public ClientsDamagableExplosionApplyControllersSynchronizer(
            IClientsSynchronizersMediator clientsSynchronizersMediator,
            DamageReceiversRepository damageReceiversesRepository, DamageDealersRepository damageDealersesRepository,
            PushablesRepository pushablesRepository, ConfigsProviderService configsProviderService)
        {
            clientsSynchronizersMediator
                .SubscribeToBroadcast<CreateAndApplyDamagableExplosionApplyControllerBroadcast>(
                    HandleBroadcastAsync);

            return;

            async void HandleBroadcastAsync(CreateAndApplyDamagableExplosionApplyControllerBroadcast broadcast,
                Channel channel)
            {
                var controller = new DamagableExplosionApplyController(new ExplosionForceApplier(),
                    new DamageReceiversFinder(damageReceiversesRepository,
                        damageDealersesRepository.ValueByKey[broadcast.DamageDealerObjectId], () => broadcast.ExplosionData.Position),
                    pushablesRepository, (await configsProviderService.GetConfigAsync<PhysicsLayersConfig>()).LayerMaskByLayerGroup[
                        PhysicsLayerGroup.Environment]);
                controller.Explode(broadcast.ExplosionData);
            }
        }
    }
}