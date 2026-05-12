using FishNet.Object;
using FishNet.Object.Synchronizing;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.SynchronizerBehaviours;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers
{
    public class NetworkVector3SynchronizerBehaviour : NetworkBehaviour, IValueUpdater<Vector3>
    {
        public NetworkValueSynchronizerBehaviour<Vector3> Synchronizer { get; private set; }
        private readonly SyncVar<Vector3> _value = new();
        
        public override void OnStartNetwork()
        {
            Synchronizer = NetworkValueSynchronizerBehaviourTools.GetCreated(_value, this, this);
        }
        
        public async void UpdateValueAsync(Vector3 value)
        {
            await AsyncTools.WaitWhileWithoutSkippingFrame(() => !IsClientInitialized);
            UpdateValueServerRpc(value);
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdateValueServerRpc(Vector3 value)
        {
            _value.Value = value;
        }
    }
}