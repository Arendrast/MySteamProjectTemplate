using FishNet.Managing.Client;
using FishNet.Transporting;
using ProjectName.LevelModule.Runtime.Shared.Scripts;
using ProjectName.LevelModule.Runtime.Shared.Scripts.Network;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart;
using ProjectName.SharedModule.Runtime.Client.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.LevelModule.Runtime.Client.Scripts
{
    public class ClientEnvironmentSynchronizer : IMatchClientService
    {
        private readonly LevelZoneFactory _levelZoneFactory;
        private readonly ClientManager _clientManager;

        public ClientEnvironmentSynchronizer(LevelZoneFactory levelZoneFactory, ClientManager clientManager,
            IOwnerSynchronizersMediator mediator)
        {
            _levelZoneFactory = levelZoneFactory;
            _clientManager = clientManager;

            mediator.SubscribeToBroadcast<InitializeLevelZoneBroadcast>(InitializeEnvironmentAsync);
        }

        private async void InitializeEnvironmentAsync(InitializeLevelZoneBroadcast broadcast, Channel channel)
        {
            await _levelZoneFactory.TryInitializeClientLevelZoneAsync(_clientManager
                .TryGetNetworkObjectById(broadcast.EnvironmentNetworkObjectId)
                .GetComponent<LevelZoneSerializableComponents>());
        }
    }
}