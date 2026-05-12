using FishNet.Object;
using FishNet.Object.Synchronizing;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.SynchronizerBehaviours
{
    public class NetworkBoolSynchronizerBehaviour : NetworkBehaviour, IValueUpdater<bool>
    {
        public NetworkValueSynchronizerBehaviour<bool> Synchronizer { get; private set; }
        private readonly SyncVar<bool> _value = new();
        
        public override void OnStartNetwork()
        {
            Synchronizer = NetworkValueSynchronizerBehaviourTools.GetCreated(_value, this, this);
        }
        
        public async void UpdateValueAsync(bool value)
        {
            await AsyncTools.WaitWhileWithoutSkippingFrame(() => !IsClientInitialized);
            UpdateValueServerRpc(value);
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdateValueServerRpc(bool value)
        {
            _value.Value = value;
        }
    }
}