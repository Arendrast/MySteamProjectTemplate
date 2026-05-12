using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Despawners
{
    public class DespawnerAfterEndParticleSystem : MonoBehaviour
    {
        [SerializeField] private bool _shouldWaitAndForLifeTime;

        private async void Start()
        {
            var mostLongParticleSystem = GetComponentsInChildren<ParticleSystem>()
                .OrderByDescending(system => system.main.duration)
                .FirstOrDefault();

            if (mostLongParticleSystem == null)
                return;

            if (await AsyncTools.AwaitTaskAndGetDoesThrowOperationCancelledException(
                    UniTask.WaitForSeconds(
                        mostLongParticleSystem.main.duration + (_shouldWaitAndForLifeTime
                            ? mostLongParticleSystem.main.startLifetime.constantMax
                            : 0f),
                        cancellationToken: destroyCancellationToken)))
            {
                return;
            }

            InstanceFinder.ServerManager.TryDespawnOrDestroyAsync(gameObject);
        }
    }
}