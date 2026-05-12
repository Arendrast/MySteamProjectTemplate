#if WWISE
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Sounds
{
    public class AkPostEventSenderOnEnable : MonoBehaviour
    {
        [SerializeField] private string _eventName;
        
        private void OnEnable()
        {
            AkSoundEngine.PostEvent(_eventName, gameObject);
        }
    }
}
#endif