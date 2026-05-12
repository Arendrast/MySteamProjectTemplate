using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Animations
{
    [Serializable]
    public class AnimancerStateEventsConfig
    {
        [field: SerializeField]
        public IReadOnlyList<IAnimancerStateEventConfig> Events { get; private set; }
            = new List<IAnimancerStateEventConfig>();
    }
}