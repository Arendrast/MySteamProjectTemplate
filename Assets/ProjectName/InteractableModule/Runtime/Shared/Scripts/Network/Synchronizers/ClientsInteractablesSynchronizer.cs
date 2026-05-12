using Cysharp.Threading.Tasks;
using FishNet.Managing.Client;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Serialization;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using Channel = FishNet.Transporting.Channel;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Synchronizers
{
    public class ClientsInteractablesSynchronizer : IMatchSharedService
    {
        private readonly InteractablesFactory _interactablesFactory;
        private readonly ClientManager _clientManager;
        private readonly AcceptInteractableBroadcastForClientHandleService _handleService;
        private readonly IClientsRemoteInteractionVisitorsProvider _visitorsProvider;

        public ClientsInteractablesSynchronizer(
            InteractablesFactory interactablesFactory, ClientManager clientManager,
            AcceptInteractableBroadcastForClientHandleService handleService,
            IClientsRemoteInteractionVisitorsProvider visitorsProvider,
            IClientsSynchronizersMediator mediator)
        {
            _interactablesFactory = interactablesFactory;
            _clientManager = clientManager;
            _handleService = handleService;
            _visitorsProvider = visitorsProvider;

            mediator.SubscribeToBroadcast<InitializeInteractablesBroadcast>(HandleInitializeInteractablesBroadcastAsync);
            mediator.SubscribeToBroadcast<AcceptInteractableBroadcastForClient>(HandleAcceptInteractableBroadcast);
        }

        private void HandleAcceptInteractableBroadcast(AcceptInteractableBroadcastForClient broadcast, Channel channel)
        {
            _handleService.HandleAcceptInteractableBroadcastAsync(broadcast,
                _visitorsProvider.NotOwnersInteractionVisitors[broadcast.FromNetworkConnection]);
        }

        private async void HandleInitializeInteractablesBroadcastAsync(InitializeInteractablesBroadcast broadcast,
            Channel channel)
        {
            for (var i = 0; i < broadcast.Data.Length; i++)
            {
                var data = broadcast.Data[i];
                var serializableComponents = _clientManager.TryGetNetworkObjectById(data.NetworkObjectId)
                    .gameObject.GetComponent<InteractableSerializableComponents>();

                _interactablesFactory.GetCreatedInteractableAsync(serializableComponents,
                    await data.SerializedConcreteInteractableData
                        .GetFromJsonDeserializedWithoutNullsAsync() as IInteractableInitializationData,
                    canInteract: broadcast.CanInteractByInteractable[i]).Forget();
            }
        }
    }
}