#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectName.DebugModule.Shared.Scripts
{
    // Для использования typeof

    public class LayerFinder : EditorWindow
    {
        private int targetLayer = 0;

        [MenuItem("Tools/Find Objects By Physics Layer")]
        public static void ShowWindow()
        {
            GetWindow<LayerFinder>("Layer Finder");
        }

        private void OnGUI()
        {
            GUILayout.Label("Поиск объектов по слою", EditorStyles.boldLabel);
        
            // Поле для ввода номера слоя
            targetLayer = EditorGUILayout.IntField("Номер слоя (ID):", targetLayer);
        
            // Показываем имя слоя для справки, чтобы не ошибиться
            string layerName = LayerMask.LayerToName(targetLayer);
            EditorGUILayout.HelpBox($"Слой {targetLayer} называется: {(string.IsNullOrEmpty(layerName) ? "---" : layerName)}", MessageType.Info);

            EditorGUILayout.Space();

            // Кнопка 1: Поиск во всем проекте (префабы)
            if (GUILayout.Button("Найти во всем проекте (Assets)"))
            {
                FindInProject(targetLayer);
            }

            // Кнопка 2: Поиск внутри выделенного объекта (Hierarchy)
            if (GUILayout.Button("Найти внутри выделенного (Selection)"))
            {
                FindInSelection(targetLayer);
            }
        }

        // ЛОГИКА 1: Поиск во всем проекте
        private void FindInProject(int layer)
        {
            string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");
            List<GameObject> foundObjects = new List<GameObject>();

            foreach (string guid in allPrefabGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab != null)
                {
                    // Проверяем сам корень префаба и всех его детей
                    GameObject[] allChildren = GetAllChildren(prefab);
                    foreach (GameObject obj in allChildren)
                    {
                        if (obj.layer == layer)
                        {
                            foundObjects.Add(obj);
                            Debug.Log($"[Project] Найден объект: {obj.name} в префабе: {path}", obj);
                        }
                    }
                }
            }

            ShowResultsCount(foundObjects.Count);
        }

        // ЛОГИКА 2: Поиск внутри выделенного
        private void FindInSelection(int layer)
        {
            if (Selection.activeGameObject == null)
            {
                Debug.LogWarning("Ничего не выбрано в Hierarchy!");
                return;
            }

            GameObject root = Selection.activeGameObject;
            Transform[] allChildren = root.GetComponentsInChildren<Transform>(true);
            List<GameObject> foundObjects = new List<GameObject>();

            foreach (Transform child in allChildren)
            {
                if (child.gameObject.layer == layer)
                {
                    foundObjects.Add(child.gameObject);
                    Debug.Log($"[Selection] Найден объект: {child.name}", child.gameObject);
                }
            }

            ShowResultsCount(foundObjects.Count);
        }

        // Вспомогательный метод получения всех дочерних объектов (для префабов)
        private GameObject[] GetAllChildren(GameObject root)
        {
            Transform[] ts = root.GetComponentsInChildren<Transform>(true);
            GameObject[] objs = new GameObject[ts.Length];
            for (int i = 0; i < ts.Length; i++)
                objs[i] = ts[i].gameObject;
            return objs;
        }

        private void ShowResultsCount(int count)
        {
            if (count == 0)
                Debug.Log("Объекты с таким слоем не найдены.");
            else
                Debug.Log($"Поиск завершен. Найдено объектов: {count}. (Кликните на сообщение в консоли, чтобы выделить объект)");
        }
    }
}
#endif