#if ADDRESSABLES
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Loading
{
    public static class AssetProvider
    {
        public static async UniTask<T> InstantiateAsync<T>(string assetId,
            IAssetLoader assetLoader,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null,
            bool shouldMakeDontDestroyOnLoad = false,
            Type type = null,
            string name = null,
            bool shouldAppointParentAfterInstantiate = false) where T : Object
        {
            var spawnParent = shouldAppointParentAfterInstantiate ? null : parent;

            var instance = await assetLoader.GetInstantiatedGameObjectAsync(assetId, position, rotation, spawnParent);

            if (shouldAppointParentAfterInstantiate)
                instance.transform.SetParent(parent, false);

            if (typeof(T) == typeof(GameObject))
                return instance as T;

            var targetComponent = instance.GetComponent<T>(type, assetId);

            if (shouldMakeDontDestroyOnLoad)
                Object.DontDestroyOnLoad(targetComponent);

            if (name != null)
                instance.name = name;

            return targetComponent;
        }

#if ADDRESSABLES
        public static async UniTask<T> InstantiateAsync<T>(AssetReference assetReference,
            IAssetLoader assetLoader,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null,
            bool shouldMakeDontDestroyOnLoad = false,
            Type type = null,
            string name = null,
            bool shouldAppointParentAfterInstantiate = false) where T : Object =>
            await InstantiateAsync<T>(await assetReference.GetAssetIdAsync(),
                assetLoader,
                position,
                rotation,
                parent,
                shouldMakeDontDestroyOnLoad,
                type,
                name,
                shouldAppointParentAfterInstantiate);
#endif

        public static async UniTask<IEnumerable<T>> LoadAllAsync<T>(string assetLabelId, IAssetLoader assetLoader,
            DataContainer<Action> releaseActionContainer)
            where T : Object
        {
            return await assetLoader.GetLoadedAssetsAsync<T>(assetLabelId, releaseActionContainer);
        }

        public static async UniTask<Object> LoadAsync(string assetId, IAssetLoader assetLoader,
            DataContainer<Action> releaseActionContainer, Type type = null)
        {
            if (type == typeof(Sprite))
                return await LoadAsync<Sprite>(assetId, assetLoader, releaseActionContainer);

            return await LoadAsync<Object>(assetId, assetLoader, releaseActionContainer, type);
        }

        public static async UniTask<T> LoadAsync<T>(AssetReference assetReference, IAssetLoader assetLoader,
            DataContainer<Action> releaseActionContainer, Type type = null)
            where T : Object
        {
            return await LoadAsync<T>(await assetReference.GetAssetIdAsync(), assetLoader, releaseActionContainer, type);
        }
        
        public static async UniTask<T> LoadAsync<T>(string assetId, IAssetLoader assetLoader,
            DataContainer<Action> releaseActionContainer, Type type = null)
            where T : Object
        {
            if (string.IsNullOrEmpty(assetId))
                return null;
            
            try
            {
                if (typeof(T).IsSubclassOf(typeof(Component)))
                    return GetComponent<T>(
                        await assetLoader.GetLoadedAssetAsync<GameObject>(assetId, releaseActionContainer),
                        type ?? typeof(T),
                        assetId);

                var @object = await assetLoader.GetLoadedAssetAsync<T>(assetId, releaseActionContainer);
                
                if (!@object)
                    return null;

                if (type == null || type.IsInstanceOfType(@object))
                    return @object;

                if (@object is not GameObject gameObject)
                {
                    Debug.LogError($"Object with type {type} and assetId {assetId} isn't found");
                    return null;
                }

                return GetComponent<T>(gameObject, type, assetId);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                return null;
            }
        }

        private static T GetComponent<T>(this GameObject gameObject, Type type, string assetId) where T : class
        {
            try
            {
                T targetComponent;

                if (type == null)
                    gameObject.TryGetComponent(out targetComponent);
                else
                {
                    var components = gameObject.GetComponents<Component>();
                    
                    targetComponent = components.FirstOrDefault(type.IsInstanceOfType) as T;
                }

                if (targetComponent == null)
                    throw new NullReferenceException(
                        $"Component {type ?? typeof(T)} on gameObject with assetId {assetId} not found");

                return targetComponent;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }

            return null;
        }
    }
}

// Arendrast