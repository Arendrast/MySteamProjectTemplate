using MoreLinq;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class ParticleSystemsPlayOnEnableStopOnDisableMaker : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particleSystems;
        
        private void OnEnable()
        {
            _particleSystems.ForEach(system => system.Play());
        }

        private void OnDisable()
        {
            _particleSystems.ForEach(system => system.Stop());
        }
    }
}
