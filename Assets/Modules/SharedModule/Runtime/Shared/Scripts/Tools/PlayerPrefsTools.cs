#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class PlayerPrefsTools
    {
        [MenuItem("Tools/PlayerPrefs/DeleteAll")]
        public static void RemoveAllKeysForPlayerPrefs() => PlayerPrefs.DeleteAll();
    }
}
#endif