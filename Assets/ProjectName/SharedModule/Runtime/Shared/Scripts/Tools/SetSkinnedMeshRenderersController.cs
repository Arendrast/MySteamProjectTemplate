using System;
using System.Collections.Generic;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Rendering;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public class SetSkinnedMeshRenderersController
    {
        private Dictionary<string, Transform> _boneMap;

        private readonly Transform _parent;
        private readonly Transform _skeletonRoot;
        private readonly Func<GameObject> _getSkinnedMeshRenderersPrefabFunc;
        private readonly Transform _root;
        private readonly List<GameObject> _spawnedObjects = new();

        private bool _isInitialized;
        private readonly RenderingLayersConfig _renderingLayersConfig;

        public SetSkinnedMeshRenderersController(Transform parent, Transform skeletonRoot, 
            Transform root, RenderingLayersConfig renderingLayersConfig, Func<GameObject> getSkinnedMeshRenderersPrefabFunc)
        {
            _parent = parent;
            _skeletonRoot = skeletonRoot;
            _root = root;
            _renderingLayersConfig = renderingLayersConfig;
            _getSkinnedMeshRenderersPrefabFunc = getSkinnedMeshRenderersPrefabFunc;
        }

        public void OnDisable()
        {
            foreach (var obj in _spawnedObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }

        public void TryInitialize()
        {
            if (_isInitialized)
            {
                EnableSpawnedObjects();
                AppointRenderingLayerMask();
                return;
            }

            var prefab = _getSkinnedMeshRenderersPrefabFunc.Invoke();

            if (prefab == null)
            {
                AppointRenderingLayerMask();
                return;
            }

            BuildBoneMap();
            SpawnAndBind(prefab);
            _isInitialized = true;

            AppointRenderingLayerMask();
        }

        public void EnableSpawnedObjects()
        {
            foreach (var obj in _spawnedObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }

        private void AppointRenderingLayerMask()
        {
            foreach (var skinnedMeshRenderer in _root.GetComponentsInChildren<SkinnedMeshRenderer>())
                skinnedMeshRenderer.renderingLayerMask = _renderingLayersConfig[RenderingLayerGroup.RealtimeLight];
        }

        private void BuildBoneMap()
        {
            if (_skeletonRoot == null) return;

            _boneMap = new Dictionary<string, Transform>();
            foreach (var t in _skeletonRoot.GetComponentsInChildren<Transform>())
                _boneMap[t.name] = t;
        }

        private void SpawnAndBind(GameObject prefab)
        {
            var instance = Object.Instantiate(prefab, _parent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;

            if (_boneMap == null || _boneMap.Count == 0)
            {
                _spawnedObjects.Add(instance);
                return;
            }

            var renderers = instance.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (renderers.Length == 0)
            {
                _spawnedObjects.Add(instance);
                return;
            }

            foreach (var smr in renderers)
            {
                RebindBones(smr);
                smr.transform.SetParent(_parent, false);
                _spawnedObjects.Add(smr.gameObject);
            }

            Object.Destroy(instance);
        }

        private void RebindBones(SkinnedMeshRenderer smr)
        {
            var oldBones = smr.bones;
            var newBones = new Transform[oldBones.Length];

            for (int i = 0; i < oldBones.Length; i++)
            {
                if (oldBones[i] != null && _boneMap.TryGetValue(oldBones[i].name, out var mapped))
                    newBones[i] = mapped;
            }

            smr.bones = newBones;

            if (smr.rootBone != null && _boneMap.TryGetValue(smr.rootBone.name, out var newRoot))
                smr.rootBone = newRoot;
        }
    }
}