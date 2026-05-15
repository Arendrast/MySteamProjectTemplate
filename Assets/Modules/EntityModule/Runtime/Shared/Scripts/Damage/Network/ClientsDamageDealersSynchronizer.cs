using FishNet.Transporting;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage.Network.Broadcasts;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Damage.Network
{
    public class ClientsDamageDealersSynchronizer : IMatchSharedService
    {
        public ClientsDamageDealersSynchronizer(
            DamageReceiversRepository damageReceiversModelIndexRepository,
            IClientsSynchronizersMediator otherClientsSynchronizersMediator)
        {
            otherClientsSynchronizersMediator.SubscribeToBroadcast<DealDamageBroadcast>(TryHandleDamageAsync);

            return;

            async void TryHandleDamageAsync(DealDamageBroadcast broadcast, Channel channel)
            {
                DamageReceiverModel receiver = null;

                if ((receiver =
                        await damageReceiversModelIndexRepository.GetValueByKeyOrWaitUntilAddAsync(broadcast
                            .ReceiverNetworkObjectId)) is null)
                {
                    return;
                }

                Debug.Log(
                    $"[DamageBroadcast] ReceiverObjectId={broadcast.ReceiverNetworkObjectId}, dmg={broadcast.DoDamageData.Damage}");
                receiver.TryTakeDamage(broadcast.DoDamageData, out var tookDamage);
            }
        }
    }
}