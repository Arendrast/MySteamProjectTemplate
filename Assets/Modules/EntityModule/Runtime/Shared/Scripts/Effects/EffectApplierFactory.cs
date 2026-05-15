using FishNet.Managing.Server;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Network;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects
{
    public class EffectApplierFactory : IMatchSharedFactory
    {
        private readonly EffectablesRepository _effectablesRepository;
        private readonly DoEffectActionForNetworkObjectSynchronizationService _synchronizationService;
        private readonly ServerManager _serverManager;

        public EffectApplierFactory(EffectablesRepository effectablesRepository, DoEffectActionForNetworkObjectSynchronizationService synchronizationService, ServerManager serverManager)
        {
            _effectablesRepository = effectablesRepository;
            _synchronizationService = synchronizationService;
            _serverManager = serverManager;
        }

        public EffectApplierController GetCreatedEffectApplierController(
            EffectApplierSerializableComponents effectApplierInstance, EffectType effectType, int effectApplierId,
            float? lifeTime = null,
            float? timeBeforeCancelEffect = null, OverlapObserver overlapObserver = null)
        {
            return new EffectApplierController(effectApplierInstance,
                _effectablesRepository, effectType, _synchronizationService, _serverManager, lifeTime, timeBeforeCancelEffect, effectApplierId, overlapObserver);
        }
    }
}