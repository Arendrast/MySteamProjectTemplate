#if WWISE
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts
{
    public class PostEventPlayer : MonoBehaviour
    {
        public void PostEvent(string eventName)
        {
            AkSoundEngine.PostEvent(string.IsNullOrEmpty(eventName) ? "null" : eventName, gameObject);
        }
    }
}
#endif