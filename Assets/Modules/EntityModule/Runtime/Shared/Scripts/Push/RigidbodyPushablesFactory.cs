using FishNet.Managing.Server;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public class RigidbodyPushablesFactory : IMatchSharedFactory
    {
        private readonly PushablesRepository _explodables;
        private readonly ServerManager _serverManager;

        public RigidbodyPushablesFactory(PushablesRepository explodables, ServerManager serverManager)
        {
            _explodables = explodables;
            _serverManager = serverManager;
        }

        public void TryCreateRigidbodyPushHandler(
            ExplodableSerializableComponents explodableSerializableComponents)
        {
            if (!_serverManager.Started || _explodables.ValueByKey.TryGetValue(explodableSerializableComponents, out var explodable) ||
                !explodableSerializableComponents.TryGetComponent<Rigidbody>(out var rigidbody))
                return;

            _explodables.Add(explodableSerializableComponents, new RigidbodyPushHandler(rigidbody));

            explodableSerializableComponents.GetOrAddComponent<EnableDisableObserver>().Disabled +=
                Dispose;

            return;

            void Dispose()
            {
                _explodables.RemoveByKey(explodableSerializableComponents);
            }
        }
    }
}