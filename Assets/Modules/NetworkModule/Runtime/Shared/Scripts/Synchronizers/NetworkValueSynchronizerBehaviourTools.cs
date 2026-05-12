using FishNet.Object;
using FishNet.Object.Synchronizing;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.SynchronizerBehaviours;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers
{
    public static class NetworkValueSynchronizerBehaviourTools
    {
        public static NetworkValueSynchronizerBehaviour<TValue> GetCreated<TValue>(SyncVar<TValue> syncVar,
            NetworkBehaviour networkBehaviour, IValueUpdater<TValue> valueUpdater)

        {
            var synchronizer = new NetworkValueSynchronizerBehaviour<TValue>(() => networkBehaviour.Owner.IsLocalClient,
                 valueUpdater, () => syncVar.Value);
            synchronizer.OnStartNetwork();
            return synchronizer;
        }
    }
}