using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Loading;
using UnityEngine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts
{
    public class PersistentServicesScopeFactory
    {
        private const string PersistentServicesScopeAssetId = "PersistentServicesScope";

        public void CreatePersistentServicesScope() => AssetProvider
            .InstantiateAsync<GameObject>(PersistentServicesScopeAssetId, AssetsLoaderTools.GetAssetLoader()).Forget();
    }
}