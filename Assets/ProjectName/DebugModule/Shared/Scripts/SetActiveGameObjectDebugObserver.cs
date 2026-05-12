using UnityEngine;

namespace ProjectName.DebugModule.Shared.Scripts
{
    public class SetActiveGameObjectDebugObserver : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log($"Enabled {name}", gameObject);
        }
        
        private void OnDisable()
        {
            Debug.Log($"Disabled {name}", gameObject);
        }
    }
}