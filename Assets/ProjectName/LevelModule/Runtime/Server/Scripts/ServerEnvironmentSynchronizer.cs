using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Object;
using ProjectName.LevelModule.Runtime.Shared.Scripts;
using ProjectName.LevelModule.Runtime.Shared.Scripts.Network;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Scene;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ServerSynchronizerPart;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.LevelModule.Runtime.Server.Scripts
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