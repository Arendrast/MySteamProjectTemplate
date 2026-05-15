using FishNet.Managing.Client;
using FishNet.Transporting;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Network
{
    public class ClientsEffectApplyNetworkObjectSynchronizer : IMatchSharedService
    {
        public ClientsEffectApplyNetworkObjectSynchronizer(
            IClientsSynchronizersMediator clientsSynchronizersMediator,
            EffectablesRepository explodableRepository, ClientManager clientManager)
        {
            clientsSynchronizersMediator
                .SubscribeToBroadcast<ApplyOrCancelEffectBroadcast>(
                    HandleBroadcastAsync);

            return;

            async void HandleBroadcastAsync(ApplyOrCancelEffectBroadcast broadcast, Channel channel)
            {
                var effectableSerializableComponents = clientManager.TryGetNetworkObjectById(broadcast.ReceiverId)
                    ?.GetComponent<EffectableSerializableComponents>();

                IEffectable effectable = null;
                
                if (effectableSerializableComponents == null ||
                    (effectable = await explodableRepository.GetValueByKeyOrWaitUntilAddAsync(effectableSerializableComponents)) is null)
                {
                    return;
                }

                switch (broadcast.EffectActionType)
                {
                    case EffectActionType.Cancel:
                        effectable.TryCancelEffect(broadcast.EffectType, broadcast.ApplierId,
                            broadcast.TimeBeforeCancel);
                        break;
                    case EffectActionType.Apply:
                        effectable.TryApplyEffect(broadcast.EffectType, broadcast.ApplierId, broadcast.EffectOrigin);
                        break;
                    default:
                        return;
                }

                Debug.Log(
                    $"Effect Action Type: {broadcast.EffectActionType}, Receiver: {broadcast.ReceiverId}, Effect: {broadcast.EffectType}, EffectOrigin: {broadcast.EffectOrigin}");
            }
        }
    }
}