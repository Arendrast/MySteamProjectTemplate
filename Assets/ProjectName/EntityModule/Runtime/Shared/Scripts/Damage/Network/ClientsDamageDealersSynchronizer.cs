using FishNet.Transporting;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage.Network.Broadcasts;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Damage.Network
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