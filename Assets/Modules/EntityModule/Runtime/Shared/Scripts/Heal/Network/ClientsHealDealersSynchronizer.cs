using FishNet.Transporting;
using Modules.EntityModule.Runtime.Shared.Scripts.Heal.Network.Broadcasts;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Heal.Network
{
    public class ClientsHealDealersSynchronizer : IMatchSharedService
    {
        public ClientsHealDealersSynchronizer(
            HealReceiversRepository damageReceiversModelIndexRepository,
            IClientsSynchronizersMediator clientsSynchronizersMediator)
        {
            clientsSynchronizersMediator.SubscribeToBroadcast<DealHealBroadcast>(HandleHeal);

            return;

            void HandleHeal(DealHealBroadcast broadcast, Channel channel)
            {
                if (!damageReceiversModelIndexRepository.ValueByKey.TryGetValue(broadcast.ReceiverNetworkObjectId,
                        out var receiver))
                {
                    Debug.LogWarning($"Heal receiver is not found. Id: {broadcast.ReceiverNetworkObjectId}");
                    return;
                }
                
                receiver.TryHeal(broadcast.DoHealData);
            }
        }
    }
}