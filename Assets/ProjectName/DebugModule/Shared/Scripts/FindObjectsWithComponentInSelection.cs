#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ProjectName.DebugModule.Shared.Scripts
{
    public class FindObjectsWithComponentInSelection : EditorWindow
    {
        private string targetComponentName = "";
        private List<GameObject> foundObjects = new List<GameObject>();

        [MenuItem("Tools/Find Objects By Component")]
        public static void ShowWindow()
        {
            GetWindow<FindObjectsWithComponentInSelection>("Find Component in Selection");
        }

        private void OnGUI()
        {
            GUILayout.Label("Find Objects with Component in Selection", EditorStyles.boldLabel);

            // Поле для ввода названия компонента
            targetComponentName = EditorGUILayout.TextField("Component Name:", targetComponentName);

            if (GUILayout.Button("Find Objects in Selection"))
            {
                FindObjects();
            }

            if (foundObjects.Count > 0)
            {
                GUILayout.Space(10);
                GUILayout.Label($"Found {foundObjects.Count} objects:", EditorStyles.miniBoldLabel);
                foreach (GameObject obj in foundObjects)
                {
                    if (obj != null) // Проверка на случай, если объект был удален
                    {
                        if (GUILayout.Button(obj.name, EditorStyles.linkLabel))
                        {
                            Selection.activeGameObject = obj; // Выделить найденный объект в иерархии
                            EditorGUIUtility.PingObject(obj); // Подсветить объект в окне Project
                        }
                    }
                }
            }
        }

        private void FindObjects()
        {
            foundObjects.Clear();

            if (string.IsNullOrEmpty(targetComponentName))
            {
                Debug.LogWarning("Component Name cannot be empty.");
                return;
            }

            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogWarning("Please select a GameObject in the Hierarchy.");
                return;
            }

            System.Type componentType = null;
            try
            {
                // Пытаемся найти тип компонента по имени.
                componentType = Type.GetType(targetComponentName + ",Assembly-CSharp") ?? Type.GetType(targetComponentName);
                if (componentType == null)
                {
                    Debug.LogError($"Component '{targetComponentName}' not found. Make sure the name is correct and it's a valid Unity script or built-in component.");
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error getting component type '{targetComponentName}': {e.Message}");
                return;
            }

            // Получаем все дочерние объекты, включая сам выделенный объект
            GameObject[] allDescendants = selectedObject.GetComponentsInChildren<Transform>(true)
                .Select(t => t.gameObject)
                .ToArray();

            foreach (GameObject obj in allDescendants)
            {
                if (obj.GetComponent(componentType) != null)
                {
                    foundObjects.Add(obj);
                }
            }

            if (foundObjects.Count == 0)
            {
                Debug.Log($"No objects found with component '{targetComponentName}' within the selection '{selectedObject.name}'.");
            }
            else
            {
                Debug.Log($"Found {foundObjects.Count} objects with component '{targetComponentName}' within the selection '{selectedObject.name}'.");
            }
        }
    }
}
#endif