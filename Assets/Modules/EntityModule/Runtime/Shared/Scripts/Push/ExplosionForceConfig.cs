using System;
using Animancer.Units;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    [Serializable]
    public class ExplosionForceConfig
    {
        [field: SerializeField] [field: Meters] public float ForceAtCenter { get; private set; }
        [field: SerializeField] [field: Meters] public float ForceAtEdge { get; private set; }
        [field: SerializeField] [field: Meters] public float Radius { get; private set; }

        public ExplosionForceData ToData()
        {
            return new ExplosionForceData(ForceAtCenter, ForceAtEdge, Radius);
        }
    }
}