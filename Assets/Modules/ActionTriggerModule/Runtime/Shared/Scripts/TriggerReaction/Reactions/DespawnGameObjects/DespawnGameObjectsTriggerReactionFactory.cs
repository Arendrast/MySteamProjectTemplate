using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.DespawnGameObjects
{
    public class DespawnGameObjectsTriggerReactionFactory : ConcreteActionTriggerReactionFactory<
        DespawnGameObjectsTriggerReactionConfig>
    {
        private readonly ServerManager _serverManager;

        public DespawnGameObjectsTriggerReactionFactory(ServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        public override UniTask<IActionTriggerReaction> GetConcreteReactionAsync(DespawnGameObjectsTriggerReactionConfig config)
        {
            return new UniTask<IActionTriggerReaction>(new DespawnGameObjectsTriggerReaction(config, _serverManager));
        }
    }
}