using FishNet.Managing.Client;
using FishNet.Object;
using FishNet.Transporting;
using Newtonsoft.Json;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Serialization;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Synchronizers
{
    public class OwnerInteractablesSynchronizer : IMatchSharedService
    {
        private readonly InteractablesRepository _interactablesesRepository;
        private readonly IOwnerInteractionObserverProvider _ownerInteractionObserverProvider;
        private readonly IOwnerInteractionControllerProvider _ownerInteractionControllerProvider;
        private readonly ClientManager _clientManager;

        public OwnerInteractablesSynchronizer(
            InteractablesRepository interactablesesRepository,
            ClientManager clientManager,
            IOwnerInteractionObserverProvider ownerInteractionObserverProvider,
            IOwnerInteractionControllerProvider ownerInteractionControllerProvider,
            IOwnerSynchronizersMediator ownerSynchronizersMediator)
        {
            _interactablesesRepository = interactablesesRepository;
            _clientManager = clientManager;
            _ownerInteractionObserverProvider = ownerInteractionObserverProvider;
            _ownerInteractionControllerProvider = ownerInteractionControllerProvider;
            
            ownerSynchronizersMediator.SubscribeToBroadcast<TakeAcceptInteractableResultBroadcastForClient>(HandleAcceptInteractableBroadcastAsync);
            ownerSynchronizersMediator.SubscribeToAction(SubscribeAfterInitialize, Unsubscribe, true);
        }

        private void SubscribeAfterInitialize()
        {
            _ownerInteractionObserverProvider.InteractionObserver.StartedInteraction += SendAcceptInteractableBroadcastAsync;
            _ownerInteractionObserverProvider.InteractionObserver.CancelledApprovedInteraction +=
                SendCancelInteractionBroadcast;
        }

        private void Unsubscribe()
        {
            if (_ownerInteractionControllerProvider.InteractionController != null)
            {
                _ownerInteractionObserverProvider.InteractionObserver.StartedInteraction -=
                    SendAcceptInteractableBroadcastAsync;
                _ownerInteractionObserverProvider.InteractionObserver.CancelledApprovedInteraction -=
                    SendCancelInteractionBroadcast;
            }
        }

        private async void HandleAcceptInteractableBroadcastAsync(TakeAcceptInteractableResultBroadcastForClient broadcast,
            Channel channel)
        {
            if (broadcast.Successfully)
                _ownerInteractionControllerProvider.InteractionController.TryInteractWithTargetInteractable(
                    await broadcast.SerializedInteractionData
                        .GetFromJsonDeserializedWithoutNullsAsync() as IFromServerInteractionData);
            else
                _ownerInteractionControllerProvider.InteractionController.CancelInteractionWithTargetInteractable();
        }

        private void SendCancelInteractionBroadcast(IInteractable interactable)
        {
            if (interactable is not IClientSyncableInteractable syncableInteractable)
                return;

            _clientManager.Broadcast(new CancelInteractionBroadcastForServer(_interactablesesRepository
                .KeyByValue[interactable].GetComponent<NetworkObject>().ObjectId));
        }

        private async void SendAcceptInteractableBroadcastAsync(IInteractable interactable,
            IAdditionalInteractionData additionalInteractionData)
        {
            if (interactable is not IClientSyncableInteractable syncableInteractable)
                return;

            _clientManager.Broadcast(new AcceptInteractableBroadcastForServer(new InteractableData(
                _interactablesesRepository.KeyByValue[interactable].GetComponent<NetworkObject>().ObjectId,
                await additionalInteractionData.GetJsonSerializedObjectWithoutNullsAsync(TypeNameHandling.All))));
        }
    }
}