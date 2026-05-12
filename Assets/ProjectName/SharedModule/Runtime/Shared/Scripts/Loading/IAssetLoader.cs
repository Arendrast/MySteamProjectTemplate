using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Loading
{
    public interface IAssetLoader
    {
        UniTask<GameObject> GetInstantiatedGameObjectAsync(string assetId, Vector3? position = null, Quaternion? rotation = null, Transform parent = null);

        UniTask<T> GetLoadedAssetAsync<T>(string assetId, DataContainer<Action> releaseActionContainer) where T : Object;

        UniTask<IEnumerable<T>> GetLoadedAssetsAsync<T>(string labelOrFolderPath, DataContainer<Action> releaseActionContainer) where T : Object;
    }
}