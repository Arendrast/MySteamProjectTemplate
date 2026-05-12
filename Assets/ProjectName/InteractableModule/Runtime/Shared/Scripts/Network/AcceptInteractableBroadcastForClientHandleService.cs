using FishNet.Managing.Client;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Serialization;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Network
{
    public class AcceptInteractableBroadcastForClientHandleService : IMatchSharedService
    {
        private readonly ClientManager _clientManager;
        private readonly InteractablesRepository _interactablesesRepository;

        public AcceptInteractableBroadcastForClientHandleService(ClientManager clientManager, InteractablesRepository interactablesesRepository)
        {
            _clientManager = clientManager;
            _interactablesesRepository = interactablesesRepository;
        }

        public async void HandleAcceptInteractableBroadcastAsync(AcceptInteractableBroadcastForClient broadcast, INotOwnerInteractionVisitor visitor)
        {
            var interactableSerializableComponents = _clientManager
                .TryGetNetworkObjectById(broadcast.InteractableData.NetworkObjectId)?
                .GetComponent<InteractableSerializableComponents>();

            if (interactableSerializableComponents == null || !_interactablesesRepository.ValueByKey.TryGetValue(
                    interactableSerializableComponents,
                    out var interactable) || interactable is not IClientSyncableInteractable syncanbleInteractable)
                return;
            
            syncanbleInteractable.Accept(
                visitor, await broadcast.InteractableData
                    .SerializedConcreteInteractableData
                    .GetFromJsonDeserializedWithoutNullsAsync() as IAdditionalInteractionData);

            if (syncanbleInteractable is IRemovableInteractable removableInteractable && removableInteractable.ShouldBeRemoved())
                _interactablesesRepository.RemoveByKey(interactableSerializableComponents);
        }
    }
}