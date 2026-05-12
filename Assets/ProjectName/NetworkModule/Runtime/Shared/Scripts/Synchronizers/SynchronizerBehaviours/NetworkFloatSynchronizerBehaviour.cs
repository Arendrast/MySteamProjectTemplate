using FishNet.Object;
using FishNet.Object.Synchronizing;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.SynchronizerBehaviours
{
    public class NetworkFloatSynchronizerBehaviour : NetworkBehaviour, IValueUpdater<float>
    {
        public NetworkValueSynchronizerBehaviour<float> Synchronizer { get; private set; }
        private readonly SyncVar<float> _value = new();
        
        public override void OnStartNetwork()
        {
            Synchronizer = NetworkValueSynchronizerBehaviourTools.GetCreated(_value, this, this);
        }
        
        public async void UpdateValueAsync(float value)
        {
            await AsyncTools.WaitWhileWithoutSkippingFrame(() => !IsClientInitialized);
            UpdateValueServerRpc(value);
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdateValueServerRpc(float value)
        {
            _value.Value = value;
        }
    }
}