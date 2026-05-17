using Cysharp.Threading.Tasks;
using Modules.AppModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.Loading;

namespace Modules.AppModule.Runtime.Shared.Scripts
{
    public class PersistentServicesScopeFactory
    {
        private const string PersistentServicesScopeAssetId = "PersistentServicesScope";

        public async UniTask<PersistentServicesScope> CreatePersistentServicesScopeAsync() => await AssetProvider
            .InstantiateAsync<PersistentServicesScope>(PersistentServicesScopeAssetId, AssetsLoaderTools.GetAssetLoader());
    }
}