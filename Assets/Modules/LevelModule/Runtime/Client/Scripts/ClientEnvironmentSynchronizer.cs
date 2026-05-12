using FishNet.Managing.Client;
using FishNet.Transporting;
using Modules.LevelModule.Runtime.Shared.Scripts;
using Modules.LevelModule.Runtime.Shared.Scripts.Network;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart;
using Modules.SharedModule.Runtime.Client.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.LevelModule.Runtime.Client.Scripts
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