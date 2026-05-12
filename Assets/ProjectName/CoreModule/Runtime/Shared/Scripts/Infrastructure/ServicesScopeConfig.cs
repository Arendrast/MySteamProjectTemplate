using System;
using UnityEngine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    [Serializable]
    public struct ServicesScopeConfig
    {
        [field: SerializeField] public bool ShouldUseBuildMode { get; private set; }
    }
}