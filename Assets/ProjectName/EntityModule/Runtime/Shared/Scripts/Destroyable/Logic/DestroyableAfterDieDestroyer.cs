using System;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Destroyable.Logic
{
    public class DestroyableAfterDieDestroyer
    {
        private readonly DestructionStatesConfig _statesConfig;
        private readonly GameObject _destroyableGameObject;
        private readonly ServerManager _serverManager;

        public DestroyableAfterDieDestroyer(DestructionStatesConfig statesConfig,
            GameObject destroyableGameObject, ServerManager serverManager)
        {
            _statesConfig = statesConfig;
            _destroyableGameObject = destroyableGameObject;
            _serverManager = serverManager;
        }

        public async void TryDespawnAfterTime()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_statesConfig.DestroyConfig.TimeBeforeDestroyAfterDie),
                cancellationToken: _destroyableGameObject.GetCancellationTokenOnDestroy());
            
            _serverManager.TryDespawnOrDestroyAsync(_destroyableGameObject);
        }
    }
}