using UnityEngine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
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
