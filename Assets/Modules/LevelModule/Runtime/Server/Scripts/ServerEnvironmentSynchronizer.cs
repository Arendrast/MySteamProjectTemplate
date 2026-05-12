using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Object;
using Modules.LevelModule.Runtime.Shared.Scripts;
using Modules.LevelModule.Runtime.Shared.Scripts.Network;
using Modules.NetworkModule.Runtime.Shared.Scripts.Scene;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.LevelModule.Runtime.Server.Scripts
{
    public class ServerEnvironmentSynchronizer : IMatchServerService
    {
        private readonly ServerSceneManagementService _serverSceneManagementService;
        private readonly ServerManager _serverManager;
        private readonly LevelZoneRepository _levelZoneRepository;

        public ServerEnvironmentSynchronizer(ServerSceneManagementService serverSceneManagementService,
            ServerManager serverManager, IServerSynchronizersMediator mediator,
            LevelZoneRepository levelZoneRepository)
        {
            _serverSceneManagementService = serverSceneManagementService;
            _serverManager = serverManager;
            _levelZoneRepository = levelZoneRepository;

            mediator.SubscribeToAction(Subscribe, Unsubscribe);
        }

        public void Subscribe()
        {
            _serverSceneManagementService.AddedConnectionToScene += SendInitializeEnvironmentAsync;
        }

        public void Unsubscribe()
        {
            _serverSceneManagementService.AddedConnectionToScene -= SendInitializeEnvironmentAsync;
        }

        private async void SendInitializeEnvironmentAsync(NetworkConnection connection)
        {
            await AsyncTools.WaitWhileWithoutSkippingFrame(() =>
                _levelZoneRepository.TargetLevelZoneSerializableComponents == null);

            _serverManager.Broadcast(connection,
                new InitializeLevelZoneBroadcast(_levelZoneRepository
                    .PersistentObjectsLevelZoneSerializableComponents.gameObject
                    .GetComponent<NetworkObject>().ObjectId));

            _serverManager.Broadcast(connection,
                new InitializeLevelZoneBroadcast(_levelZoneRepository
                    .TargetLevelZoneSerializableComponents.gameObject
                    .GetComponent<NetworkObject>().ObjectId));
        }
    }
}