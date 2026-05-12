using System.Collections.Generic;
using Modules.SharedModule.Runtime.Shared.Scripts.Index;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Damage
{
    public struct DoDamageData
    {
        public readonly int Damage;
        public readonly Vector3? DamageDirection;
        public readonly List<CustomTag> Tags;
        public readonly int DamageDealerId;
        public readonly DamageOrigin DamageOrigin;

        public DoDamageData(int damage, DamageOrigin damageOrigin, Vector3? damageDirection = null, List<CustomTag> tags = null, int damageDealerId = IndexableTools.MissingOrInvalidId)
        {
            DamageDirection = damageDirection;
            Tags = tags;
            Damage = damage;
            DamageOrigin = damageOrigin;
            DamageDealerId = damageDealerId;
        }
        
        public DoDamageData(int damage, DamageOrigin damageOrigin)
        {
            DamageDirection = null;
            Tags = null;
            Damage = damage;
            DamageOrigin = damageOrigin;
            DamageDealerId = IndexableTools.MissingOrInvalidId;
        }

        public DoDamageData WithDamageDealerId(int damageDealerId)
        {
            return new DoDamageData(Damage, DamageOrigin, DamageDirection, Tags, damageDealerId);
        }
    }
}