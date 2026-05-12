using FishNet.Managing.Server;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.DespawnGameObjects
{
    public class DespawnGameObjectsTriggerReaction : IActionTriggerReaction
    {
        private readonly DespawnGameObjectsTriggerReactionConfig _reactionConfig;
        private readonly ServerManager _serverManager;

        public DespawnGameObjectsTriggerReaction(DespawnGameObjectsTriggerReactionConfig reactionConfig, ServerManager serverManager)
        {
            _reactionConfig = reactionConfig;
            _serverManager = serverManager;
        }

        public void Invoke()
        {
            foreach (var gameObject in _reactionConfig.GameObjects)
            {
                if (_reactionConfig.ShouldDespawnOnlyGameObjectsChildren)
                {
                    foreach (Transform child in gameObject.transform)
                    {
                        _serverManager.TryDespawnOrDestroyAsync(child.gameObject);
                    }
                }
                else
                {
                    _serverManager.TryDespawnOrDestroyAsync(gameObject);
                }
            }
        }
    }
}