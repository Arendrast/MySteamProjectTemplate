using System;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using FishNet.Object;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Network;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using MoreLinq;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects
{
    public class EffectApplierController
    {
        public EffectApplierSerializableComponents SerializableComponents { get; }

        private readonly float _lifeTime;
        private readonly float _timeBeforeCancelEffect;

        private readonly long _startDespawnTimeInTicks;
        private readonly int _effectApplierId;
        private readonly EffectablesRepository _effectablesRepository;
        private readonly EffectType _effectType;
        private readonly DoEffectActionForNetworkObjectSynchronizationService _synchronizationService;

        public EffectApplierController(EffectApplierSerializableComponents serializableComponents,
            EffectablesRepository effectablesRepository, EffectType effectType,
            DoEffectActionForNetworkObjectSynchronizationService synchronizationService, ServerManager serverManager,
            float? lifeTime, float? timeBeforeCancelEffect, int effectApplierId)
        {
            SerializableComponents = serializableComponents;
            _effectablesRepository = effectablesRepository;
            _effectType = effectType;
            _synchronizationService = synchronizationService;
            _effectApplierId = effectApplierId;
            _lifeTime = lifeTime ?? serializableComponents.LifeTime;
            _timeBeforeCancelEffect = timeBeforeCancelEffect ?? serializableComponents.TimeBeforeCancelEffect;
            serializableComponents.OverlapObserver.Entered += TryApplyEffectOnEnter;
            serializableComponents.OverlapObserver.Exited += TryCancelEffectOnExit;

            if (_lifeTime <= 0)
            {
                serializableComponents.OverlapObserver.CurrentOverlaps.ForEach(TryApplyEffectOnEnter);
                return;
            }

            _startDespawnTimeInTicks = DateTime.Now.Ticks;

            serializableComponents.OverlapObserver.CurrentOverlaps.ForEach(TryApplyEffectOnEnter);

            Timer.TryStartCountingTime(_lifeTime,
                () => serverManager.TryDespawnOrDestroyAsync(serializableComponents.gameObject), true,
                serializableComponents.GetCancellationTokenOnDestroy()).Forget();
        }

        private void TryApplyEffectOnEnter(Collider collider)
        {
            if (!collider.TryGetComponentInParentsByPredicate<EffectableSerializableComponents>(
                    out var effectableSerializableComponents) ||
                !_effectablesRepository.TryGetValue(effectableSerializableComponents, out var effectable)) return;

            effectable.TryApplyEffect(_effectType, _effectApplierId, EffectOrigin.EffectApplier);

            if (effectableSerializableComponents.TryGetComponentInParentsByPredicate<NetworkObject>(
                    out var networkObject))
            {
                _synchronizationService.SendEffectActionData(new EffectActionData(_effectType, networkObject.ObjectId,
                    _effectApplierId, EffectActionType.Apply, EffectOrigin.EffectApplier, 0));
            }

            if (_startDespawnTimeInTicks == 0) return;

            TryCancelEffect(effectableSerializableComponents, effectable,
                _lifeTime - _startDespawnTimeInTicks.GetPastTimeInSeconds());
        }

        private void TryCancelEffectOnExit(Collider collider)
        {
            if (!collider.TryGetComponentInParentsByPredicate<EffectableSerializableComponents>(
                    out var effectableSerializableComponents) ||
                !_effectablesRepository.TryGetValue(effectableSerializableComponents, out var effectable)) return;

            TryCancelEffect(effectableSerializableComponents, effectable,
                _timeBeforeCancelEffect);
        }

        private void TryCancelEffect(EffectableSerializableComponents effectableSerializableComponents,
            IEffectable effectable, float delay)
        {
            effectable.TryCancelEffect(_effectType, _effectApplierId, delay);

            if (effectableSerializableComponents.TryGetComponentInParentsByPredicate<NetworkObject>(
                    out var networkObject))
                _synchronizationService.SendEffectActionData(new EffectActionData(_effectType, networkObject.ObjectId,
                    _effectApplierId, EffectActionType.Cancel, EffectOrigin.EffectApplier, delay));
        }
    }
}