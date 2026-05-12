using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using MoreLinq;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Damage
{
    public class DamageReceiversFinder
    {
        private readonly DamageReceiversRepository _damageReceiversesRepository;
        private readonly DamageDealerModel _damageDealerModel;
        private readonly Func<Vector3> _damageOrigin;

        public DamageReceiversFinder(DamageReceiversRepository damageReceiversesRepository,
            DamageDealerModel damageDealerModel, Func<Vector3> damageOrigin)
        {
            _damageReceiversesRepository = damageReceiversesRepository;
            _damageDealerModel = damageDealerModel;
            _damageOrigin = damageOrigin;
        }

        public void TryDoDamage(IEnumerable<Component> components, int damage, DamageOrigin damageOrigin)
        {
            components.ForEach(component => TryDoDamage(component, damage, damageOrigin));
        }

        public void TryDoDamage(Component component, int damage, DamageOrigin damageOrigin)
        {
            TryDoDamage(component, damage, out var damagedId, damageOrigin);
        }

        public void TryDoDamage(Component component, int damage, out int damagedId, DamageOrigin damageOrigin)
        {
            damagedId = -1;

            if (component == null)
            {
                return;
            }

            DamageReceiverModel damageReceiver = null;
            
            var networkObject =
                component.GetComponentInParentsByPredicate<NetworkObject>(networkObjec => _damageReceiversesRepository
                    .ValueByKey.TryGetValue(networkObjec.ObjectId, out damageReceiver));

            if (damageReceiver == null)
            {
                return;
            }
            
            List<CustomTag> tags = null;

            if (component.TryGetComponent<DamageMultiplierZone>(out var zone))
                damage = Mathf.RoundToInt(damage * zone.DamageMultiplier);

            if (component.TryGetComponent<Taggable>(out var taggable))
                tags = taggable.Tags?.ToList();
                
            damagedId = networkObject.ObjectId;
            _damageDealerModel.DoDamage(damageReceiver,
                new DoDamageData(damage, damageOrigin, _damageOrigin.Invoke() - networkObject.transform.position, tags));
        }
    }
}