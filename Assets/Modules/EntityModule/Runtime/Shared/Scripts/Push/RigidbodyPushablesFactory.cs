using FishNet.Managing.Server;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public class RigidbodyPushablesFactory : IMatchSharedFactory
    {
        private readonly PushablesRepository _explodableses;
        private readonly ServerManager _serverManager;

        public RigidbodyPushablesFactory(PushablesRepository explodableses, ServerManager serverManager)
        {
            _explodableses = explodableses;
            _serverManager = serverManager;
        }

        public void TryCreateRigidbodyPushHandler(
            ExplodableSerializableComponents explodableSerializableComponents)
        {
            if (!_serverManager.Started || _explodableses.ValueByKey.TryGetValue(explodableSerializableComponents, out var explodable) ||
                !explodableSerializableComponents.TryGetComponent<Rigidbody>(out var rigidbody))
                return;

            _explodableses.Add(explodableSerializableComponents, new RigidbodyPushHandler(rigidbody));

            explodableSerializableComponents.GetOrAddComponent<DisableObserver>().Disabled +=
                Dispose;

            return;

            void Dispose()
            {
                _explodableses.RemoveByKey(explodableSerializableComponents);
            }
        }
    }
}