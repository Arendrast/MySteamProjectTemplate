using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    [Serializable]
    public struct FloatRange
    {
        [field: SerializeField] public float Minimal { get; private set; }
        [field: SerializeField] public float Maximal { get; private set; }

        public FloatRange(float minimal, float maximal)
        {
            Minimal = minimal;
            Maximal = maximal;
        }
    }
}