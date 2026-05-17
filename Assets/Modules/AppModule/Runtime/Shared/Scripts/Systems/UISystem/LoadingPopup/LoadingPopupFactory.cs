using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.AppModule.Runtime.Shared.Scripts.Systems.UISystem.LoadingPopup
{
    public class LoadingPopupFactory : IPersistentFactory
    {
        private readonly HashedAssetProvider _hashedAssetProvider;

        private const string LoadingPopupAssetId = "LoadingPopup";   
        
        public LoadingPopupFactory(HashedAssetProvider hashedAssetProvider)
        {
            _hashedAssetProvider = hashedAssetProvider;
        }

        public void Dispose()
        {
            DisposeAsync().Forget();
        }

        public async UniTask DisposeAsync()
        {
            await _hashedAssetProvider.DisposeAsync();
        }

        public async UniTask<LoadingPopupController> GetLoadingPopupControllerAsync()
        {
            return await _hashedAssetProvider
                .GetControllerAsync<LoadingPopupController, LoadingPopupSerializableComponents>(
                    LoadingPopupAssetId,
                    popup =>
                    {
                        _hashedAssetProvider.RegisterAndGetSingleByType(new LoadingPopupController());
                        return UniTask.CompletedTask;
                    },
                    shouldMakeDontDestroyOnLoad: true
                );
        }
    }
}