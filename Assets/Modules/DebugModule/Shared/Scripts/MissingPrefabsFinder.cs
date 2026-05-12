#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Modules.DebugModule.Shared.Scripts
{
    public static class MissingPrefabsFinder
    {
        [MenuItem("Tools/Find missing prefabs in selected gameobjects")]
        public static void Find()
        {
            var selectedGameObjects = Selection.gameObjects;

            foreach (var gameObject in selectedGameObjects)
            {
                foreach (var childTransform in gameObject.GetComponentsInChildren<Transform>(true))
                {
                    var currentGameObject = childTransform.gameObject;

                    var components = currentGameObject.GetComponents<Component>();

                    if (components.Any(component => component == null))
                        Debug.Log($"{currentGameObject.name} имеет отсутствующий скрипт", currentGameObject);
                }
            }
        }
    }
}
#endif