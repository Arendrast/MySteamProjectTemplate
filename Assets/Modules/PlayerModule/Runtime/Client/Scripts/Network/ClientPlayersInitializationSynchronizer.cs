using Cysharp.Threading.Tasks;
using FishNet.Managing.Client;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.Network.Broadcasts;
using Modules.PlayerModule.Runtime.Shared.Scripts.Operator;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using MoreLinq;
using Channel = FishNet.Transporting.Channel;

namespace Modules.PlayerModule.Runtime.Client.Scripts.Network
{
    public class ClientPlayersInitializationSynchronizer : IMatchSharedService
    {
        private readonly OwnerPlayerFactory _ownerPlayerFactory;
        private readonly ClientsPlayersFactory _clientsPlayersFactory;
        private readonly ClientManager _clientManager;
        private readonly IsOperatorRepository _isOperatorRepository;

        public ClientPlayersInitializationSynchronizer(
            OwnerPlayerFactory ownerPlayerFactory,
            ClientManager clientManager, ClientsPlayersFactory clientsPlayersFactory,
            IClientsSynchronizersMediator clientsSynchronizersMediator,
            IsOperatorRepository isOperatorRepository)
        {
            _ownerPlayerFactory = ownerPlayerFactory;
            _clientManager = clientManager;
            _clientsPlayersFactory = clientsPlayersFactory;
            _isOperatorRepository = isOperatorRepository;

            clientsSynchronizersMediator.SubscribeToBroadcast<InitializePlayersBroadcast>(InitializePlayers);
            clientsSynchronizersMediator.SubscribeToBroadcast<InitializePlayerBroadcast>(InitializePlayer);
            clientsSynchronizersMediator.SubscribeToBroadcast<IsOperatorQuestionBroadcast>(SendIsOperatorAnswerBroadcast);
        }

        private void SendIsOperatorAnswerBroadcast(IsOperatorQuestionBroadcast broadcast, Channel channel)
        {
            _clientManager.Broadcast(new IsOperatorAnswerBroadcast(_isOperatorRepository.IsOperator));
        }

        private void InitializePlayers(InitializePlayersBroadcast playersBroadcast, Channel channel)
        {
            playersBroadcast.InitializePlayerMessages.ForEach(broadcast =>
                InitializePlayer(broadcast, channel));
        }

        private void InitializePlayer(InitializePlayerBroadcast broadcast, Channel channel)
        {
            var playerInitializationData = new ClientsPlayersFactory.ClientPlayerInitializationData(
                broadcast.InitializeInventoryItemsData,
                broadcast.ViewNetworkObjectId,
                broadcast.StateType, broadcast.TargetInteractableData,
                broadcast.CharacterType);

            var connection = _clientManager.GetNetworkConnectionByClientId(broadcast.OwnerNetworkConnectionId);

            if (connection.IsOwner(_clientManager))
            {
                _ownerPlayerFactory.GetCreatedOwnerPlayerComponentsAsync(
                        connection,
                        _clientManager.TryGetNetworkObjectById(broadcast.GameObjectNetworkObjectId)
                            .GetComponent<OwnerPlayerSerializableComponents>(), playerInitializationData)
                    .Forget();
            }
            else
            {
                _clientsPlayersFactory
                    .GetCreatedClientPlayerComponentsAsync(connection,
                        _clientManager.TryGetNetworkObjectById(broadcast.GameObjectNetworkObjectId)
                            .GetComponent<ClientPlayerSerializableComponents>(),
                        playerInitializationData)
                    .Forget();
            }
        }
    }
}