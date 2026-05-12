using FishNet.Managing.Client;
using FishNet.Transporting;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

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
                    HandleBroadcast);

            return;

            void HandleBroadcast(ApplyOrCancelEffectBroadcast broadcast, Channel channel)
            {
                var effectableSerializableComponents = clientManager.TryGetNetworkObjectById(broadcast.ReceiverId)
                    ?.GetComponent<EffectableSerializableComponents>();
                
                if (effectableSerializableComponents == null ||
                    !explodableRepository.TryGetValue(effectableSerializableComponents, out var effectable))
                    return;
                
                switch (broadcast.EffectActionType)
                {
                    case EffectActionType.Cancel:
                        effectable.TryCancelEffect(broadcast.EffectType, broadcast.ApplierId, broadcast.TimeBeforeCancel);
                        break;
                    case EffectActionType.Apply:
                        effectable.TryApplyEffect(broadcast.EffectType, broadcast.ApplierId, broadcast.EffectOrigin);
                        break;
                }
            }
        }
    }
}