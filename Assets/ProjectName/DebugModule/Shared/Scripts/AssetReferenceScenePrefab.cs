using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProjectName.DebugModule.Shared.Scripts
{
    [Serializable]
    public class AssetReferenceScenePrefab : AssetReferenceT<GameObject>
    {
        public AssetReferenceScenePrefab(string guid) : base(guid) { }

#if UNITY_EDITOR
        public override bool ValidateAsset(UnityEngine.Object obj)
        {
            var go = obj as GameObject;
            return go != null && go.GetComponent<ScenePrefab>() != null;
        }

        public override bool ValidateAsset(string path)
        {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return go != null && go.GetComponent<ScenePrefab>() != null;
        }
#endif
    }
}