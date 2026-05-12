using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Object;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ProjectName.DebugModule.Shared.Scripts
{
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class ScenePrefab : MonoBehaviour
    {
        [field: SerializeField] public int NumberLoadedAssetsByFrame { get; private set; }
        [SerializeField] private ObjectInfo[] m_objects;
        [SerializeField] private bool _shouldDestroyAfterCollect;

        private List<AsyncOperationHandle<GameObject>> _spawnedObjectHandles = new();
        private float _loadProgress;

        public float LoadProgress => _loadProgress;
        public event Action OnLoadCompleted;

#if UNITY_EDITOR
        [ContextMenu("Collect Addressable Children")]
        private void Collect()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("Addressables Settings not found. Create Addressables settings first.");
                return;
            }

            var list = new List<ObjectInfo>();

            var objectsForDestroy = new List<GameObject>();

            foreach (var t in GetComponentsInChildren<Transform>(false))
            {
                if (t == transform) continue;

                var prefabAsset = PrefabUtility.GetCorrespondingObjectFromOriginalSource(t.gameObject);
                if (prefabAsset == null) continue;

                var path = AssetDatabase.GetAssetPath(prefabAsset);
                if (string.IsNullOrEmpty(path)) continue;

                string guid = AssetDatabase.AssetPathToGUID(path);
                AddressableAssetEntry entry = settings.FindAssetEntry(guid);
                if (entry == null) continue;

                list.Add(new ObjectInfo
                {
                    AssetRefGO = new AssetReferenceGameObject(entry.guid),
                    Position = t.position,
                    Rotation = t.rotation,
                    Scale = t.lossyScale
                });

                objectsForDestroy.Add(t.gameObject);
            }

            if (_shouldDestroyAfterCollect)
            {
                foreach (var gameObject in objectsForDestroy)
                {
                    DestroyImmediate(gameObject);
                }
            }

            Undo.RecordObject(this, "Collect Addressable Children");
            m_objects = list.ToArray();
            EditorUtility.SetDirty(this);

            Debug.Log($"Collected {m_objects.Length} addressable children.");
        }
#endif


        public async UniTask LoadAsync(CancellationToken ct = default, IProgress<float> progress = null)
        {
            _loadProgress = 0;

            if (_spawnedObjectHandles.Count > 0)
                Unload();

            int total = m_objects?.Length ?? 0;
            if (total == 0)
            {
                _loadProgress = 1f;
                progress?.Report(_loadProgress);
                return;
            }

            int completed = 0;

            for (int i = 0; i < total; i++)
            {
                ObjectInfo obj = m_objects[i];

                var gameObjec = await InstantiateAsync(obj, ct, gameObject.transform);

                if (gameObjec != null && gameObjec.TryGetComponent<NetworkObject>(out var networkObject))
                {
                    InstanceFinder.ServerManager.TryCustomSpawn(networkObject);
                }

                completed++;
                _loadProgress = (float)completed / total;
                progress?.Report(_loadProgress);

                if (completed % NumberLoadedAssetsByFrame == 0)
                    await UniTask.DelayFrame(1, cancellationToken: ct);
            }

            _loadProgress = 1f;
            progress?.Report(_loadProgress);
        }

        private async UniTask<GameObject> InstantiateAsync(ObjectInfo obj, CancellationToken ct, Transform parent)
        {
            var handle = Addressables.InstantiateAsync(obj.AssetRefGO, obj.Position, obj.Rotation, parent);
            _spawnedObjectHandles.Add(handle);

            try
            {
                GameObject go = await handle.ToUniTask(cancellationToken: ct);

                if (go != null)
                {
                    go.transform.localScale = obj.Scale;
                }
                else
                {
                    Debug.LogError($"InstantiateAsync returned null for key: {obj.AssetRefGO}");
                }

                return go;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"InstantiateAsync failed for key: {obj.AssetRefGO}\n{e}");
            }

            return null;
        }

        public void Unload()
        {
            foreach (AsyncOperationHandle<GameObject> handle in _spawnedObjectHandles)
            {
                Addressables.ReleaseInstance(handle);
            }

            _spawnedObjectHandles.Clear();
        }

        [Serializable]
        public class ObjectInfo
        {
            public AssetReferenceGameObject AssetRefGO;
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Scale;
        }
    }
}