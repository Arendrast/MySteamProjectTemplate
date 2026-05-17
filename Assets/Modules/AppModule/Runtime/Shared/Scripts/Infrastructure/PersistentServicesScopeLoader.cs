using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.AppModule.Runtime.Shared.Scripts.Infrastructure
{
    public class PersistentServicesScopeLoader : MonoBehaviour
    {
        private bool _createdScope;

        private async void Awake()
        {
            await UniTask.DelayFrame(1, cancellationToken: destroyCancellationToken);
            await TryCreatePersistentServicesScopeAsync();
        }

        public async UniTask<PersistentServicesScope> TryCreatePersistentServicesScopeAsync()
        {
            if (_createdScope)
            {
                return null;
            }

            _createdScope = true;
            var value = await new PersistentServicesScopeFactory().CreatePersistentServicesScopeAsync();
            Destroy(gameObject);
            
            return value;
        }
    }
}
