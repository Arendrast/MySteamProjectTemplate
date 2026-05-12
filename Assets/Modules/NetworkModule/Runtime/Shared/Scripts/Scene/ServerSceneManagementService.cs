using System;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Managing.Server;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using SceneManager = FishNet.Managing.Scened.SceneManager;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Scene
{
    public class ServerSceneManagementService : IMatchServerService
    {
        public SceneLoadData ActiveSceneLoadData { get; private set; }
        public UnityEngine.SceneManagement.Scene ActiveScene { get; private set; }
        public event Action<NetworkConnection> LoadedSceneClient, UnloadedSceneClient, AddedConnectionToScene;

        private readonly SceneManager _sceneManager;
        private readonly ServerManager _serverManager;

        public ServerSceneManagementService(SceneManager sceneManager, ServerManager serverManager)
        {
            _sceneManager = sceneManager;
            _serverManager = serverManager;
        }

        public void TryInvokingClientLoadedScene(ClientPresenceChangeEventArgs args)
        {
            if (args.Added)
                LoadedSceneClient?.Invoke(args.Connection);
            else
                UnloadedSceneClient?.Invoke(args.Connection);
        }

        public void AddConnectionToActiveScene(NetworkConnection networkConnection)
        {
            _sceneManager.AddConnectionToScene(networkConnection, ActiveScene);
            AddedConnectionToScene?.Invoke(networkConnection);
        }

        public async UniTask LoadNewStackedGameSceneAsync(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                sceneName);

            ActiveScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);

            var sceneLoadData = new SceneLoadData(ActiveScene)
            {
                Options = new LoadOptions
                {
                    AllowStacking = false
                },
                ReplaceScenes = ReplaceOption.All
            };

            await UniTask.WaitWhile(() => !_sceneManager.NetworkManager.ServerManager.Started);

            _sceneManager.LoadConnectionScenes(sceneLoadData);

            ActiveSceneLoadData = sceneLoadData;
        }
    }
}