using FishNet.Managing.Client;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Network
{
    public class OwnerEffectApplySynchronizer : IMatchSharedService
    {
        public OwnerEffectApplySynchronizer(
            DoEffectActionForNetworkObjectSynchronizationService service, ClientManager clientManager)
        {
            service.SentData += SendApplyBroadcast;

            return;

            void SendApplyBroadcast(EffectActionData actionData)
            {
                clientManager.Broadcast(new ApplyOrCancelEffectBroadcast(actionData.EffectType,
                    actionData.EffectableNetworkObjectId, actionData.ApplierId, actionData.EffectActionType,
                    actionData.TimeBeforeCancel, actionData.EffectOrigin));
            }
        }
    }
}