using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Damage
{
    public class DamageMultiplierZone : MonoBehaviour
    {
        public float DamageMultiplier => _damageMultiplier;
        [SerializeField, Min(0f)] private float _damageMultiplier = 1f;
    }
}
