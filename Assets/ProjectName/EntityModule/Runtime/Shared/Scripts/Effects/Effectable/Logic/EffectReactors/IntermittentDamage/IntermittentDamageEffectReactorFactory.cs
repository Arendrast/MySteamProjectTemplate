using Cysharp.Threading.Tasks;
using FishNet.Object;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic.EffectReactors.IntermittentDamage
{
    public class IntermittentDamageReactorFactory : IMatchSharedFactory
    {
        private readonly DamageReceiversRepository _damageReceiversRepository;
        private readonly DamageDealersRepository _damageDealersRepository;

        public IntermittentDamageReactorFactory(DamageReceiversRepository damageReceiversRepository, DamageDealersRepository damageDealersRepository)
        {
            _damageReceiversRepository = damageReceiversRepository;
            _damageDealersRepository = damageDealersRepository;
        }

        public async UniTask<IntermittentDamageReactor> GetReactorAsync(IntermittentDamageReactorConfig config,
            EffectableSerializableComponents effectableSerializableComponents, DamageOrigin damageOrigin)
        {
            var networkObject = effectableSerializableComponents.GetComponent<NetworkObject>();

            return new IntermittentDamageReactor(await _damageDealersRepository.GuaranteedGetValueByKeyAsync(networkObject.ObjectId),
                await _damageReceiversRepository.GuaranteedGetValueByKeyAsync(networkObject.ObjectId), config, damageOrigin);
        }
    }
}