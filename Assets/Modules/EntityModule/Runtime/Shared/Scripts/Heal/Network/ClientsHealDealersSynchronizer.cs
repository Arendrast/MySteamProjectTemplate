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
            clientsSynchronizersMediator.SubscribeToBroadcast<DealHealBroadcast>(HandleHealAsync);

            return;

            async void HandleHealAsync(DealHealBroadcast broadcast, Channel channel)
            {
                HealReceiverModel receiver = null;
                
                if ((receiver =
                        await damageReceiversModelIndexRepository.GetValueByKeyOrWaitUntilAddAsync(broadcast
                            .ReceiverNetworkObjectId)) is null)
                {
                    Debug.LogWarning($"Heal receiver is not found. Id: {broadcast.ReceiverNetworkObjectId}");
                    return;
                }
                
                receiver.TryHeal(broadcast.DoHealData);
            }
        }
    }
}