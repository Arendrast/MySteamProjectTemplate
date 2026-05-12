#if WWISE
using System;
using FishNet.Object;
using Sirenix.Utilities;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Sounds
{
    public class AkPostEventNetworkSender : NetworkBehaviour
    {
        private uint _lastSavedEventId;

        public void PostEvent(string eventName, bool saveEventId = false)
        {
            if (IsServerInitialized)
            {
                LocalPostEventObserverRPC(eventName, saveEventId);
            }
            else
            {
                LocalPostEventServerRpc(eventName, saveEventId);
            }
        }

        public void SimplePostEvent(string eventName)
        {
            PostEvent(eventName);
        }

        public void LocalPostEvent(string eventName, bool saveEventHash = false)
        {
            var eventHash = AkSoundEngine.PostEvent(string.IsNullOrEmpty(eventName) ? "null" : eventName, gameObject);

            if (saveEventHash)
                _lastSavedEventId = eventHash;
        }

        public void StopLastSavedEvent()
        {
            if (IsServerInitialized)
            {
                StopLastSavedEventObserverRPC();
            }
            else
            {
                StopLastSavedEventServerRpc();
            }
        }

        [ServerRpc]
        private void StopLastSavedEventServerRpc()
        {
            StopLastSavedEventObserverRPC();
        }

        [ObserversRpc]
        private void StopLastSavedEventObserverRPC()
        {
            LocalStopLastSavedEvent();
        }

        private void LocalStopLastSavedEvent()
        {
            AkSoundEngine.StopPlayingID(_lastSavedEventId);
        }

        [ServerRpc]
        private void LocalPostEventServerRpc(string eventName, bool saveEventId = false)
        {
            LocalPostEventObserverRPC(eventName, saveEventId);
        }

        [ObserversRpc]
        private void LocalPostEventObserverRPC(string eventName, bool saveEventId = false)
        {
            LocalPostEvent(eventName, saveEventId);
        }
    }
}
#endif