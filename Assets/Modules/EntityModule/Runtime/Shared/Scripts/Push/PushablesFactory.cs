using System;
using Cysharp.Threading.Tasks;
using Modules.OverlapModule.Runtime.Scripts;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

#if TWO_D
using ActualRigidbody = UnityEngine.Rigidbody2D;

#else
using ActualRigidbody = UnityEngine.Rigidbody;
#endif

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public enum PushableMovementType
    {
        Rigidbody,
        CharacterController
    }

    public class PushablesFactory : IMatchSharedFactory
    {
        private readonly PushablesRepository _explodables;
        private readonly ConfigsProviderService _configsProviderService;

        public PushablesFactory(
            PushablesRepository explodables,
            ConfigsProviderService configsProviderService)
        {
            _explodables = explodables;
            _configsProviderService = configsProviderService;
        }

        public async UniTask<PushHandlerController> TryCreatePushHandlerAsync(
            ExplodableSerializableComponents explodableSerializableComponents,
            bool shouldDisableCapsuleOverlapObserverWhenIsInactive,
            PushableMovementType movementType, DataContainer<PushHandlerModel> explosionModelContainer = null)
        {
            if (_explodables.ValueByKey.TryGetValue(explodableSerializableComponents, out var explodable))
            {
                return null;
            }

            var config = await _configsProviderService.GetConfigAsync<ExplodablesConfig>();

            explosionModelContainer ??= new DataContainer<PushHandlerModel>();

            explosionModelContainer.Data = new PushHandlerModel();

            var moveAction = GetMoveAction(out var movableTransform);

            var handler = new PushHandlerController(
                explodableSerializableComponents.LocalMass,
                config.TimeMultiplier, config.MinimumThrowTime, config.SpeedCurve,
                explodableSerializableComponents.GetOrAddComponent<OverlapObserver>(), explosionModelContainer.Data,
                shouldDisableCapsuleOverlapObserverWhenIsInactive, moveAction, movableTransform);

            _explodables.Add(explodableSerializableComponents, handler);

            explodableSerializableComponents.GetOrAddComponent<EnableDisableObserver>().Disabled += Dispose;

            return handler;

            void Dispose()
            {
                _explodables.RemoveByKey(explodableSerializableComponents);
            }

            Action<Vector3> GetMoveAction(out Transform movableTransform)
            {
                movableTransform = null;

#if !TWO_D
                if (movementType is PushableMovementType.CharacterController)
                {
                    return GetCharacterControllerMoveAction(explodableSerializableComponents, ref movableTransform);
                }
#endif

                if (movementType is PushableMovementType.Rigidbody)
                {
                    return GetRigidbodyMoveAction(explodableSerializableComponents, ref movableTransform);
                }

                return null;
            }
        }

        private static Action<Vector3> GetRigidbodyMoveAction(
            ExplodableSerializableComponents explodableSerializableComponents,
            ref Transform movableTransform)
        {
            if (!explodableSerializableComponents.TryGetComponent<ActualRigidbody>(
                    out var rigidbody))
            {
                return null;
            }

            movableTransform = rigidbody.transform;

#if TWO_D
            return position => rigidbody.MovePosition(position);
#else
            return rigidbody.MovePosition;
#endif
        }

#if !TWO_D
        private Action<Vector3> GetCharacterControllerMoveAction(
            ExplodableSerializableComponents explodableSerializableComponents,
            ref Transform movableTransform)
        {
            if (!explodableSerializableComponents.TryGetComponent<ManyInvokableOneFrameCharacterController>(
                    out var oneFrameCharacterController))
            {
                return null;
            }

            movableTransform = oneFrameCharacterController.transform;
            return oneFrameCharacterController.Move;
        }
#endif
    }
}