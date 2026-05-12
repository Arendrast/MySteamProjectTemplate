#if UNITY_EDITOR
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class SkinnedMeshColliderGenerator : MonoBehaviour
    {

        [field: SerializeField] private SkinnedMeshRenderer SkinnedMeshRenderer { get; set; }

        [field: SerializeField]
        [field: InlineButton(nameof(UpdateCollider), "UpdateCollider")]
        private MeshCollider MeshCollider { get; set; }

        private void UpdateCollider()
        {
            var colliderMesh = new Mesh
            {
                name = "SkinnedMesh"
            };

            var meshName = MeshCollider.name + "_Mesh.asset";
            var finalPath = Path
                .Combine(Path.GetDirectoryName(PrefabStageUtility.GetCurrentPrefabStage().assetPath), meshName)
                .Replace("\\", "/");
            AssetDatabase.CreateAsset(colliderMesh, finalPath);
            EditorUtility.SetDirty(MeshCollider.transform.root.gameObject);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            colliderMesh = AssetDatabase.LoadAssetAtPath<Mesh>(finalPath);

            SkinnedMeshRenderer.BakeMesh(colliderMesh);
            MeshCollider.sharedMesh = null;
            MeshCollider.sharedMesh = colliderMesh;
        }
    }
}
#endif
