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
            otherClientsSynchronizersMediator.SubscribeToBroadcast<DealDamageBroadcast>(TryHandleDamage);
            
            return;
            
            void TryHandleDamage(DealDamageBroadcast broadcast, Channel channel)
            {
                if (!damageReceiversModelIndexRepository.ValueByKey.TryGetValue(broadcast.ReceiverNetworkObjectId,
                        out var receiver))
                {
                    return;
                }

                Debug.Log($"[DamageBroadcast] ReceiverObjectId={broadcast.ReceiverNetworkObjectId}, dmg={broadcast.DoDamageData.Damage}");
                receiver.TryTakeDamage(broadcast.DoDamageData, out var tookDamage);
            }
        }
    }
}