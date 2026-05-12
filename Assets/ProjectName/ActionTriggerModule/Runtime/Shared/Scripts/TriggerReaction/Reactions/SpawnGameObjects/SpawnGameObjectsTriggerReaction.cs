using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Loading;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.SpawnGameObjects
{
    public class SpawnGameObjectsTriggerReaction : IActionTriggerReaction
    {
        public IReadOnlyList<GameObject> Instances { get; private set; }

        private readonly SpawnGameObjectsTriggerReactionConfig _reactionConfig;

        private readonly IAssetLoader _assetLoader;
        private readonly ServerManager _serverManager;


        public SpawnGameObjectsTriggerReaction(SpawnGameObjectsTriggerReactionConfig reactionConfig,
            IAssetLoader assetLoader, ServerManager serverManager)
        {
            _reactionConfig = reactionConfig;
            _assetLoader = assetLoader;
            _serverManager = serverManager;
        }

        public async void Invoke()
        {
            var tasks = _reactionConfig.AssetReferences.Select(async reference =>
                await AssetProvider.InstantiateAsync<GameObject>(reference, _assetLoader,
                    parent: _reactionConfig.TransformConfig.Parent));
            
            Instances = await UniTask.WhenAll(tasks);

            foreach (var instance in Instances)
            {
                if (_reactionConfig.TransformConfig.PositionTransform != null)
                {
                    instance.transform.position = _reactionConfig.TransformConfig.PositionTransform.position;
                }
                else if (_reactionConfig.TransformConfig.Parent != null)
                {
                    instance.transform.localPosition = _reactionConfig.TransformConfig.Position;
                }
                else
                {
                    instance.transform.position = _reactionConfig.TransformConfig.Position;
                }
                
                if (_reactionConfig.ShouldNetworkSpawn)
                    _serverManager.TryCustomSpawn(instance);
            }
        }
    }
}