using System;
using System.Collections.Generic;
using FishNet.Object;
using Modules.EntityModule.Runtime.Shared.Scripts.Push.Network;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public class ExplodableReceiversFinder
    {
        private readonly PushablesRepository _explodablesesRepository;
        private readonly ExplodeNetworkObjectSynchronizationService _synchronizationService;

        public ExplodableReceiversFinder(PushablesRepository explodablesesRepository,
            ExplodeNetworkObjectSynchronizationService synchronizationService)
        {
            _explodablesesRepository = explodablesesRepository;
            _synchronizationService = synchronizationService;
        }

        public void TryExplode(Component component, float moveDistance, Vector3 direction,
            Func<ExplodableSerializableComponents, bool> isBlockingExplosionFunc,
            out ExplodableSerializableComponents explodableSerializableComponents)
        {
            explodableSerializableComponents =
                component?.GetComponentInParentsByPredicate<ExplodableSerializableComponents>();

            if (explodableSerializableComponents == null)
            {
                return;
            }

            var receiver = _explodablesesRepository
                .ValueByKey.GetValueOrDefault(explodableSerializableComponents);

            var isBlockingExplosion = isBlockingExplosionFunc.Invoke(explodableSerializableComponents);

            receiver?.TryPush(moveDistance, direction, isBlockingExplosion);

            if (_synchronizationService != null &&
                explodableSerializableComponents.TryGetComponent<NetworkObject>(out var networkObject))
            {
                _synchronizationService.Send(moveDistance, direction, networkObject.ObjectId, isBlockingExplosion);
            }
        }
    }
}