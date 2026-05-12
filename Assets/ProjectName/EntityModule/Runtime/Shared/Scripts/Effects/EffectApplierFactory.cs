using FishNet.Managing.Server;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Network;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects
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
            float? timeBeforeCancelEffect = null)
        {
            return new EffectApplierController(effectApplierInstance,
                _effectablesRepository, effectType, _synchronizationService, _serverManager, lifeTime, timeBeforeCancelEffect, effectApplierId);
        }
    }
}