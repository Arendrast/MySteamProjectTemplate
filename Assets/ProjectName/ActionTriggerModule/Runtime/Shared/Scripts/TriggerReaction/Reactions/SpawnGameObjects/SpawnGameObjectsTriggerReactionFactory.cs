using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Loading;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.SpawnGameObjects
{
    public class SpawnGameObjectsTriggerReactionFactory : ConcreteActionTriggerReactionFactory<
        SpawnGameObjectsTriggerReactionConfig>
    {
        private readonly IAssetLoader _assetLoader;
        private readonly ServerManager _serverManager;

        public SpawnGameObjectsTriggerReactionFactory(IAssetLoader assetLoader, ServerManager serverManager)
        {
            _assetLoader = assetLoader;
            _serverManager = serverManager;
        }

        public override UniTask<IActionTriggerReaction> GetConcreteReactionAsync(SpawnGameObjectsTriggerReactionConfig config)
        {
            return new UniTask<IActionTriggerReaction>(new SpawnGameObjectsTriggerReaction(config, _assetLoader, _serverManager));
        }
    }
}