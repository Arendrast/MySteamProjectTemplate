using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public static class GameRunner
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ToInitializeScene()
        {
            var sceneName = SceneManager.GetActiveScene().name;
            
            // if (sceneName != ScenesNames.Initial)
            //     SceneManager.LoadScene(ScenesNames.Initial);
        }
    }
}