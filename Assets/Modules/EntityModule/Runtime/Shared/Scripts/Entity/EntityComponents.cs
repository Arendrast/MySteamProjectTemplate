using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;
using Modules.EntityModule.Runtime.Shared.Scripts.Heal;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Entity
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