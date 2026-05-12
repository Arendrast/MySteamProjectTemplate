using UnityEngine;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public class PersistentServicesScopeLoader : MonoBehaviour
    {
        private void Awake()
        {
            new PersistentServicesScopeFactory().CreatePersistentServicesScope();
            Destroy(gameObject);
        }
    }
}
