using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Loading
{
    public class AssetFromResourcesLoader : IAssetLoader
    {
        public async UniTask<GameObject> GetInstantiatedGameObjectAsync(string assetId, Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null)
        {
            var asset = await Resources.LoadAsync(assetId);

            if (asset == null)
                throw new NullReferenceException($"Asset as resources with path {assetId} not found");

            var instance = position.HasValue || rotation.HasValue
                ? UnityEngine.Object.Instantiate(asset,
                    position ?? Vector3.zero,
                    rotation ?? Quaternion.identity, parent) as GameObject
                : Object.Instantiate(asset, parent) as GameObject;

            return instance;
        }

        public async UniTask<T> GetLoadedAssetAsync<T>(string assetId, DataContainer<Action> releaseActionContainer)
            where T : Object
        {
            var request = Resources.LoadAsync(assetId);
            var asset = await request as T;

            releaseActionContainer.Data = ReleaseAsset;

            return asset;

            void ReleaseAsset()
            {
                Resources.UnloadAsset(asset);
            }
        }

        public UniTask<IEnumerable<T>> GetLoadedAssetsAsync<T>(string labelOrFolderPath,
            DataContainer<Action> releaseActionContainer) where T : Object
        {
            var assets = (IEnumerable<T>)Resources.LoadAll<T>(labelOrFolderPath);

            releaseActionContainer.Data =ReleaseAssets;

            return UniTask.FromResult(assets);

            void ReleaseAssets()
            {
                foreach (var asset in assets)
                    Resources.UnloadAsset(asset);
            }
        }
    }
}