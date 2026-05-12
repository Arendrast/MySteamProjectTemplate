using System;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.QoL
{
    [Serializable]
    public struct IntRange
    {
        [field: SerializeField] public int Minimal { get; private set; }
        [field: SerializeField] public int Maximal { get; private set; }

        public IntRange(int minimal, int maximal)
        {
            Minimal = minimal;
            Maximal = maximal;
        }
    }
}