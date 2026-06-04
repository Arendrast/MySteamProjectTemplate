#if !TWO_D
using Cysharp.Threading.Tasks;
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

        public UniTask<RigidbodyPushHandler> TryCreatePushHandlerAsync(
            ExplodableSerializableComponents explodableSerializableComponents)
        {
            if (!_serverManager.Started || _explodables.ValueByKey.TryGetValue(explodableSerializableComponents, out var explodable) ||
                !explodableSerializableComponents.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                return new UniTask<RigidbodyPushHandler>(null);
            }

            var handler = new RigidbodyPushHandler(rigidbody);
            
            _explodables.Add(explodableSerializableComponents, handler);

            explodableSerializableComponents.GetOrAddComponent<EnableDisableObserver>().Disabled +=
                Dispose;

            return UniTask.FromResult(handler);

            void Dispose()
            {
                _explodables.RemoveByKey(explodableSerializableComponents);
            }
        }
    }
}
#endif