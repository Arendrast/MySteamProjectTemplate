#if WWISE
using UnityEngine;

namespace CrazySWAT.SharedModule.Runtime.Shared.Scripts
{
    public class SingleSoundPlayer : MonoBehaviour
    {
        private uint _eventId;

        public void SinglePostEvent(string eventName)
        {
            _eventId = AkSoundEngine.PostEvent(eventName, gameObject);
        }

        public void StopSound()
        {
            AkSoundEngine.StopPlayingID(_eventId);
        }
    }
}
#endif