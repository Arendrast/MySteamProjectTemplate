using System.Linq;
using FishNet.Managing.Client;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using Channel = FishNet.Transporting.Channel;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.Network
{
    public class ClientsPlayersStatesSynchronizer : IMatchSharedService
    {
        public ClientsPlayersStatesSynchronizer(
            IClientsSynchronizersMediator clientsSynchronizersMediator, ClientManager clientManager,
            ClientsPlayersFactory clientsPlayersFactory)
        {
            clientsSynchronizersMediator.SubscribeToBroadcast<UpdatePlayerSharedStateBroadcastForClient>(TryUpdateStateAsync);

            return;

            async void TryUpdateStateAsync(UpdatePlayerSharedStateBroadcastForClient broadcast, Channel channel)
            {
                var targetConnection = clientManager.GetNetworkConnectionByClientId(broadcast.ClientId);
                
                var targetComponents = await clientsPlayersFactory.TryGetPlayerComponentsAsync(targetConnection);
                
                if (targetComponents == null)
                    return;
                
                var stateMachine = targetComponents.StateMachine;
                var state = stateMachine.Nodes.Values.First(node => node.State.GetStateType() == broadcast.StateType).State;
                stateMachine.EnterState(state);
            }
        }
    }
}