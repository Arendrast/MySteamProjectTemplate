#if WWISE
using FishNet.Object;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Sounds
{
    public class SetSwitchNetworkSender : NetworkBehaviour
    {
        public void SetSwitch(string name, string surface)
        {
            if (IsServerInitialized) 
            {
                LocalSetSwitchObserverRPC(name, surface);
            }
            else
            {
                LocalSetSwitchServerRpc(name, surface);
            }
        }
        
        [ServerRpc]
        private void LocalSetSwitchServerRpc(string name, string surface)
        {
            LocalSetSwitchObserverRPC(name, surface);
        }
        
        [ObserversRpc]
        private void LocalSetSwitchObserverRPC(string name, string surface)
        {
            LocalSetSwitch(name, surface);
        }

        private void LocalSetSwitch(string name, string surface)
        {
            AkSoundEngine.SetSwitch(name, surface, gameObject);
        }
    }
}
#endif