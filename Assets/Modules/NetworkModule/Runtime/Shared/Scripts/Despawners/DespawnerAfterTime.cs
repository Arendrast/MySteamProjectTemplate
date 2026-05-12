using Cysharp.Threading.Tasks;
using FishNet;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Despawners
{
    public class DespawnerAfterTime : MonoBehaviour
    {
        [SerializeField] private float _time;
        
        private async void Start()
        {
            if (await AsyncTools.AwaitTaskAndGetDoesThrowOperationCancelledException(
                    UniTask.WaitForSeconds(_time,
                        cancellationToken: destroyCancellationToken)))
            {
                return;
            }
            
            InstanceFinder.ServerManager?.TryDespawnOrDestroyAsync(gameObject);
        }
    }
}