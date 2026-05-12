using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Heal;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Entity
{
    public class EntityComponents
    {
        public readonly DamageDealerModel DamageDealerModel;
        public readonly HealDealerModel HealDealerModel;
        public readonly HealthModel HealthModel;
        public readonly DamageReceiverModel DamageReceiverModel;
        public readonly HealReceiverModel HealReceiverModel;
        public readonly EntitySerializableComponents SerializableComponents;
        public readonly EffectsReceiverModel EffectsReceiverModel;

        public EntityComponents(DamageDealerModel damageDealerModel,
            DamageReceiverModel damageReceiverModel,
            EntitySerializableComponents serializableComponents,
            HealthModel healthModel,
            HealReceiverModel healReceiverModel, HealDealerModel healDealerModel,
            EffectsReceiverModel effectsReceiverModel)
        {
            DamageDealerModel = damageDealerModel;
            DamageReceiverModel = damageReceiverModel;
            SerializableComponents = serializableComponents;
            HealthModel = healthModel;
            HealReceiverModel = healReceiverModel;
            HealDealerModel = healDealerModel;
            EffectsReceiverModel = effectsReceiverModel;
        }
    }
}