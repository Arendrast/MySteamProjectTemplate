#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class ChildrenSerializableComponentsProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                if (path.Contains("Modules") && path.EndsWith(".prefab"))
                {
                    var root = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    var registry = root?.GetComponent<ChildrenSerializableComponentsContainer>();

                    if (registry != null)
                    {
                        Bake(registry);
                    }
                }
            }
        }

        private static void Bake(ChildrenSerializableComponentsContainer container)
        {
            container.ClearContainedComponents();

            Stopwatch timer = Stopwatch.StartNew();
            
            var all = container.GetComponentsInChildren<Component>(true);
            
            foreach (var comp in all)
            {
                if (comp is MonoBehaviour and not ChildrenSerializableComponentsContainer)
                {
                    container.RegisterComponent(comp);
                }
            }
            
            EditorUtility.SetDirty(container);
            Debug.Log($"[Bake] Запечено {all.Length} компонентов в {container.gameObject.name} за {timer.ElapsedMilliseconds} ms");
        }
    }
}
#endif