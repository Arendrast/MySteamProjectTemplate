#if UNITY_EDITOR
using Modules.SharedModule.Runtime.Shared.Scripts.Holders;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class EditorSceneTools
    {
        [MenuItem("Scenes/Load Initial #1")]
        public static void LoadSceneLvl1Zone1() =>
            OpenScene($"Assets/Modules/CoreModule/Runtime/Shared/Scenes/Initial.unity");

        [MenuItem("Scenes/Load Metrics #2")]
        public static void OpenMetricsScene() =>
            OpenScene($"Assets/Modules/LevelModule/Runtime/Shared/Prefabs/Levels/Metrics/Metrics.unity");

        [MenuItem("Scenes/Load Lvl1Zone1 #3")]
        public static void OpenLvl1Zone1() =>
            OpenLvlZone(1, 1);

        [MenuItem("Scenes/Load Lvl1Zone2 #4")]
        public static void OpenLvl1Zone2() =>
            OpenLvlZone(1, 2);

        [MenuItem("Scenes/Load Lvl1Zone3 #5")]
        public static void OpenLvl1Zone3() =>
            OpenLvlZone(1, 3);

        [MenuItem("Scenes/Load Lvl1Zone4 #6")]
        public static void OpenLvl1Zone4() =>
            OpenLvlZone(1, 4);
        
        [MenuItem("Scenes/Load Lvl1Zone5 #7")]
        public static void OpenLvl1Zone5() =>
            OpenLvlZone(1, 5);
        
        [MenuItem("Scenes/Load Lvl1Zone6 #8")]
        public static void OpenLvl1Zone6() =>
            OpenLvlZone(1, 6);
        
        [MenuItem("Scenes/Load Lvl1Zone7 #9")]
        public static void OpenLvl1Zone7() =>
            OpenLvlZone(1, 7);

        private static void OpenLvlZone(int levelNumber, int zoneNumber) => OpenScene(
            $"Assets/Modules/LevelModule/Runtime/Shared/Prefabs/Levels/{levelNumber}/Zone{zoneNumber}/Lvl{levelNumber}_{zoneNumber}.unity");

        private static void OpenScene(string path)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(path);
            }
        }
    }
}
#endif