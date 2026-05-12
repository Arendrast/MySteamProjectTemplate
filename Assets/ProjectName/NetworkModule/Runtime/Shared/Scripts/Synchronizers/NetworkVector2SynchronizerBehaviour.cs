using FishNet.Object;
using FishNet.Object.Synchronizing;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.SynchronizerBehaviours;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers
{
    public class NetworkVector2SynchronizerBehaviour : NetworkBehaviour, IValueUpdater<Vector2>
    {
        public NetworkValueSynchronizerBehaviour<Vector2> Synchronizer { get; private set; }
        private readonly SyncVar<Vector2> _value = new();
        
        public override void OnStartNetwork()
        {
            Synchronizer = NetworkValueSynchronizerBehaviourTools.GetCreated(_value, this, this);
        }
        
        public async void UpdateValueAsync(Vector2 value)
        {
            await AsyncTools.WaitWhileWithoutSkippingFrame(() => !IsClientInitialized);
            UpdateValueServerRpc(value);
        }

        [ServerRpc]
        private void UpdateValueServerRpc(Vector2 value)
        {
            _value.Value = value;
        }
    }
}