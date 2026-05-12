using Cysharp.Threading.Tasks;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters;
using Modules.SharedModule.Runtime.Shared.Scripts.Loading;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public class NetworkCountersSynchronizerBehaviourFactory : IMatchSharedFactory
    {
        private readonly IAssetLoader _assetProvider;
        private readonly ServerManager _serverManager;
        private readonly ClientManager _clientManager;

        public NetworkCountersSynchronizerBehaviourFactory(IAssetLoader assetProvider, ServerManager serverManager, ClientManager clientManager)
        {
            _assetProvider = assetProvider;
            _serverManager = serverManager;
            _clientManager = clientManager;
        }

        public async UniTask<NetworkCountersSynchronizerBehaviour> GetSpawnedSynchronizer()
        {
            var instance = await AssetProvider.InstantiateAsync<NetworkCountersSynchronizerBehaviour>(
                "NetworkCountersSynchronizerBehaviour", _assetProvider);

            _serverManager.TryCustomSpawn(instance.gameObject, _clientManager.GetOwnerConnection());

            return instance;
        }
    }
}