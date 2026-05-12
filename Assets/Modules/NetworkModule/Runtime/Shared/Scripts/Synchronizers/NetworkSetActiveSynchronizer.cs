using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers
{
    public class NetworkSetActiveSynchronizer : NetworkBehaviour
    {
        [SerializeField] private bool _isActiveOnStart;
        [SerializeField] private bool _alwaysDisableForOwner;
        
        public bool IsActive => _isActive.Value;
        
        private readonly SyncVar<bool> _isActive = new(false,
            settings: new SyncTypeSettings(WritePermission.ServerOnly,
                ReadPermission.Observers));

        public override void OnStartNetwork()
        {
            _isActive.OnChange += SetActive;

            if (IsServerInitialized)
            {
                _isActive.Value = _isActiveOnStart;
            }
            
            SetActive(false, IsServerInitialized ? _isActiveOnStart : _isActive.Value, IsServerInitialized);
        }
        
        public void TrySetActiveState(bool? state = null)
        {
            if (IsServerInitialized)
            {
                _isActive.Value = state ?? !_isActive.Value;
            }
        }

        private void SetActive(bool prevValue, bool newValue, bool asServer)
        {
            gameObject.SetActive(IsOwner && _alwaysDisableForOwner ? false : newValue);
        }
    }
}