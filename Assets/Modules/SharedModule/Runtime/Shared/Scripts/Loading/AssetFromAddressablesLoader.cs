#if ADDRESSABLES
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Loading
{
    public class AssetFromAddressablesLoader : IAssetLoader
    {
        public async UniTask<GameObject> GetInstantiatedGameObjectAsync(string assetId, Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null)
        {
            var handle = position.HasValue || rotation.HasValue
                ? Addressables.InstantiateAsync(assetId, new InstantiationParameters(
                    position ?? Vector3.zero, rotation ?? Quaternion.identity, parent))
                : Addressables.InstantiateAsync(assetId, parent);

            var instance = await handle;

            if (!instance)
                throw new NullReferenceException($"Asset as addressable with assetId {assetId} is not found");

            instance.GetOrAddComponent<DestroyObserver>().Destroyed
                += () => Addressables.ReleaseInstance(handle);

            return instance;
        }

        public async UniTask<T> GetLoadedAssetAsync<T>(string assetId, DataContainer<Action> releaseActionContainer)
            where T : Object
        {
            var assetHandle = Addressables.LoadAssetAsync<T>(assetId);
            
            releaseActionContainer.Data = UnloadAsset;
            
            return await assetHandle;

            void UnloadAsset()
            {
                if (assetHandle.IsValid())
                    Addressables.Release(assetHandle);
            }
        }

        public async UniTask<IEnumerable<T>> GetLoadedAssetsAsync<T>(string labelOrFolderPath,
            DataContainer<Action> releaseActionContainer) where T : Object
        {
            var locations = await Addressables.LoadResourceLocationsAsync(labelOrFolderPath, typeof(T));
            var handles = locations.Select(Addressables.LoadAssetAsync<T>).ToList();

            releaseActionContainer.Data = UnloadAssets;

            return await UniTask.WhenAll(handles.Select(handle => handle.Task.AsUniTask()));

            void UnloadAssets()
            {
                foreach (var asyncOperationHandle in handles.Where(asyncOperationHandle =>
                             asyncOperationHandle.IsValid()))
                {
                    asyncOperationHandle.Release();
                }
            }
        }
    }
}
#endif