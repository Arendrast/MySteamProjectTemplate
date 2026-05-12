using System.Linq;
using FishNet.Managing.Client;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using Channel = FishNet.Transporting.Channel;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.Network
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