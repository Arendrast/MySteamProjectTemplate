using Cysharp.Threading.Tasks;
using FishNet;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.Despawners
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