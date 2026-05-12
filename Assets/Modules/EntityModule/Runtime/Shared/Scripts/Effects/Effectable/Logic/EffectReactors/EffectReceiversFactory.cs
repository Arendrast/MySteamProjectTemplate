using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FishNet.Object;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors
{
    public class EffectReceiversFactory : IMatchSharedFactory
    {
        private readonly EffectReactorsFactory _effectReactorsFactory;
        private readonly EffectablesRepository _effectablesRepository;

        public EffectReceiversFactory(EffectReactorsFactory effectReactorsFactory,
            EffectablesRepository effectablesRepository)
        {
            _effectReactorsFactory = effectReactorsFactory;
            _effectablesRepository = effectablesRepository;
        }

        public async UniTask<EffectsReceiverModel> GetEffectReceiverModel(
            EffectableSerializableComponents effectableSerializableComponents, bool isOwner)
        {
            var effectReactors = new Dictionary<EffectType, IEffectReactor>();

            foreach (var pair in effectableSerializableComponents.EffectReactorsConfigs)
            {
                effectReactors.Add(pair.EffectType,
                    await _effectReactorsFactory.GetCreatedEffectReactorAsync(pair.EffectType, pair.EffectReactorConfig,
                        effectableSerializableComponents, isOwner));
            }

            var model = new EffectsReceiverModel(effectReactors, effectableSerializableComponents.GetComponent<NetworkObject>().ObjectId);

            var controller =
                new EffectsReceiverController(
                    model, effectableSerializableComponents.GetOrAddComponent<MonoBehaviourObserver>());

            _effectablesRepository.TryAdd(effectableSerializableComponents, model);

            return model;
        }
    }
}